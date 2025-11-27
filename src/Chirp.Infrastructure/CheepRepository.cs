using Chirp.Core.DomainModel;
using Chirp.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chirp.Infrastructure
{
    public interface ICheepRepository
    {
        Task<List<CheepDTO>> GetCheeps(string? author = null);        
        Task<List<CheepDTO>> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null);        
        Task PostCheep(string text, string authorName, string authorEmail);        
        Task CreateUser(string authorName, string authorEmail);        
        Task<List<string>> GetFollowedIds(string userId);
        Task FollowUser(string currentUserId, string authorIdToFollow);
        Task UnfollowUser(string currentUserId, string authorIdToUnfollow);
        Task<bool> IsFollowing(string currentUserId, string authorId);
        Task<List<CheepDTO>> GetCheepsFromAuthorAndFollowing(int page, int pageSize, string authorName);
        Task<int> GetCheepCountFromAuthorAndFollowing(string authorName);
        Task<int> GetCheepCount(string? author = null);
    }

    public class CheepRepository : ICheepRepository
    {
        private readonly ChirpDbContext _context;
        private readonly ILogger _logger;

        public CheepRepository(ChirpDbContext context, ILoggerFactory factory)
        {
            _context = context;
            _logger = factory.CreateLogger<CheepRepository>();
        }

        public async Task<List<CheepDTO>> GetCheeps(string? author = null)
        {
            if (!string.IsNullOrEmpty(author))
            {
                var authorEntity = await _context.Authors
                    .Include(a => a.Cheeps)
                    .FirstOrDefaultAsync(a => a.UserName == author);

                if (authorEntity == null)
                    return new List<CheepDTO>();

                return authorEntity.Cheeps
                    .OrderByDescending(c => c.TimeStamp)
                    .Select(c => EntityToDTO.ToDTO(c))
                    .ToList();
            }
            else
            {
                return await _context.Cheeps 
                    .Include(c => c.Author)
                    .OrderByDescending(c => c.TimeStamp)
                    .Select(c => EntityToDTO.ToDTO(c))
                    .ToListAsync(); 
            }
        }

        public async Task<List<CheepDTO>> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null)
        {
            IQueryable<Cheep> query = _context.Cheeps.Include(c => c.Author);

            if (!string.IsNullOrEmpty(author))
                query = query.Where(c => c.Author!.UserName == author);

            return await query
                .OrderByDescending(c => c.TimeStamp)
                .Skip(pageSize * currentPage)
                .Take(pageSize)
                .Select(c => EntityToDTO.ToDTO(c))
                .ToListAsync(); 
        }

        public async Task PostCheep(string text, string authorName, string authorEmail)
        {
            await CreateUser(authorName, authorEmail); 

            var author = await _context.Authors.FirstOrDefaultAsync(a => a.UserName == authorName);
        
            if (author == null)
            {
                _logger.LogWarning("No author found for user {User}", authorName);
                return;
            }
            
            if (text.Length > 160)
            {
                _logger.LogWarning("{text} is longer than 160 chars", text);
                return;
            }

            var newCheep = new Cheep
            {
                Text = text,
                TimeStamp = DateTime.Now,
                Author = author
            };

            _context.Cheeps.Add(newCheep);
            await _context.SaveChangesAsync();
        }
        
        public async Task CreateUser(string authorName, string authorEmail)
        {
            if (string.IsNullOrEmpty(authorName))
                return;

            if (await _context.Authors.AnyAsync(a => a.UserName == authorName))
                return;

            var author = new Author
            {
                UserName = authorName,
                Email = authorEmail,
                Cheeps = new List<Cheep>()
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }
        
        public async Task FollowUser(string currentUserId, string authorIdToFollow)
        {
            var userToFollow = await _context.Authors.FindAsync(authorIdToFollow);

            var currentUser = await _context.Authors
                .Include(a => a.Following)
                .FirstOrDefaultAsync(a => a.Id == currentUserId);

            if (userToFollow == null || currentUser == null) return;

            if (!currentUser.Following.Contains(userToFollow))
            {
                currentUser.Following.Add(userToFollow);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UnfollowUser(string currentUserId, string authorIdToUnfollow)
        {
            var userToUnfollow = await _context.Authors.FindAsync(authorIdToUnfollow);

            var currentUser = await _context.Authors
                .Include(a => a.Following)
                .FirstOrDefaultAsync(a => a.Id == currentUserId);

            if (userToUnfollow == null || currentUser == null) return;

            currentUser.Following.Remove(userToUnfollow);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsFollowing(string currentUserId, string authorId)
        {
            return await _context.Authors
                .AnyAsync(a => a.Id == currentUserId && a.Following.Any(f => f.Id == authorId));
        }
        
        public async Task<List<CheepDTO>> GetCheepsFromAuthorAndFollowing(int page, int pageSize, string authorName)
        {
            var author = await _context.Authors
                .Include(a => a.Following)
                .FirstOrDefaultAsync(a => a.UserName == authorName);

            if (author == null) return new List<CheepDTO>();

            var followingIds = author.Following.Select(a => a.Id).ToList();
            followingIds.Add(author.Id);

            return await _context.Cheeps
                .Include(c => c.Author)
                .Where(c => followingIds.Contains(c.Author!.Id))
                .OrderByDescending(c => c.TimeStamp)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .Select(c => EntityToDTO.ToDTO(c))
                .ToListAsync();
        }
        
        public async Task<List<string>> GetFollowedIds(string userId)
        {
            var user = await _context.Authors
                .Include(a => a.Following)
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (user == null) return new List<string>();

            return user.Following.Select(a => a.Id).ToList();
        }
        
        public async Task<int> GetCheepCountFromAuthorAndFollowing(string authorName)
        {
            var author = await _context.Authors
                .Include(a => a.Following)
                .FirstOrDefaultAsync(a => a.UserName == authorName);

            if (author == null) return 0;

            var followingIds = author.Following.Select(a => a.Id).ToList();
            followingIds.Add(author.Id); 

            return await _context.Cheeps
                .CountAsync(c => followingIds.Contains(c.Author!.Id));
        }
        
        public async Task<int> GetCheepCount(string? author = null)
        {
            IQueryable<Cheep> query = _context.Cheeps;
    
            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(c => c.Author!.UserName == author);
            }
    
            return await query.CountAsync();
        }
    }
}