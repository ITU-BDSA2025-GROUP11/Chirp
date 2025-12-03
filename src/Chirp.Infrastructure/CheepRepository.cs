using Chirp.Core.DomainModel;
using Chirp.Core.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chirp.Infrastructure
{
    public interface ICheepRepository
    {
        Task<Author?> GetAuthorAndCheeps(string authorName);
        Task<List<CheepDTO>> GetCheeps();
        Task<Author?> GetAuthorIdAndFollowing(string userId);
        void AddCheep(Cheep cheep);
        Task<Author?> GetAuthorAndCheepsFromId(string authorId);
        Task SaveChanges();
        Task<bool> IsFollowing(string currentUserId, string authorId);
        Task<Cheep?> GetAuthorCheepAndLikes(int cheepIdToLike);
        Task<Author?> GetAuthorAndLikedCheeps(string userId);
        Task<Cheep?> GetAuthorCheepAndDislikes(int cheepIdToDislike);
        Task<Author?> GetAuthorAndDislikedCheeps(string userId);
        Task<Author?> GetAuthorNameAndFollowing(string authorName);
        IQueryable<Cheep> GetCheepsFromAuthor();
        Task<List<CheepDTO>> GetPaginatedCheeps(int currentPage, int pageSize, IQueryable<Cheep> query);
        Task<List<string>> GetFollowedIds(string userId);
        Task<int> CountCheeps(List<string> followingIds);
        IQueryable<Cheep> GetAllCheeps();
        Task<List<CheepDTO>> GetCheepsFromAuthorAndFollowing(int page, int pageSize, List<String> followingIds);
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

        public async Task<Author?> GetAuthorAndCheepsFromId(string authorId)
        {
            return await _context.Authors
                .Include(a => a.Cheeps)
                .FirstOrDefaultAsync(a => a.Id == authorId);
        }

        public async Task<Author?> GetAuthorAndCheeps(string authorName)
        {
            return await _context.Authors
                .Include(a => a.Cheeps)
                .FirstOrDefaultAsync(a => a.UserName == authorName);
        }

        public async Task<List<CheepDTO>> GetCheeps()
        {
            return await _context.Cheeps 
                .Include(c => c.Author)
                .OrderByDescending(c => c.TimeStamp)
                .Select(c => EntityToDTO.ToDTO(c))
                .ToListAsync(); 
        }

        public async Task<Author?> GetAuthorIdAndFollowing(string username)
        {
            return await _context.Authors
                .Include(a => a.Following)
                .FirstOrDefaultAsync(a => a.UserName == username);
        }

        public void AddCheep(Cheep cheep)
        {
            _context.Cheeps.Add(cheep);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsFollowing(string currentUserId, string authorId)
        {
            return await _context.Authors
                .AnyAsync(a => a.Id == currentUserId && a.Following.Any(f => f.Id == authorId));
        }
        
        public async Task<Cheep?> GetAuthorCheepAndLikes(int cheepIdToLike)
        {
            return await _context.Cheeps
                .Include(c => c.Author)
                .Include(c => c.Likes)
                .FirstOrDefaultAsync(a => a.CheepId == cheepIdToLike);
        }

        public async Task<Author?> GetAuthorAndLikedCheeps(string userId)
        {
            return await _context.Authors
                .Include(a  => a.LikedCheeps)
                .FirstOrDefaultAsync(a => a.Id == userId);
        }

        public async Task<Cheep?> GetAuthorCheepAndDislikes(int cheepIdToDislike)
        {
            return await _context.Cheeps
                .Include(c => c.Author)
                .Include(c => c.Dislikes)
                .FirstOrDefaultAsync(a => a.CheepId == cheepIdToDislike);
        }

        public async Task<Author?> GetAuthorAndDislikedCheeps(string userId)
        {
            return await _context.Authors
                .Include(a  => a.DislikedCheeps)
                .FirstOrDefaultAsync(a => a.Id == userId);
        }

        public async Task<Author?> GetAuthorNameAndFollowing(string authorName)
        {
            return await _context.Authors
                .Include(a => a.Following)
                .FirstOrDefaultAsync(a => a.UserName == authorName);
        }
        
        public async Task<List<CheepDTO>> GetCheepsFromAuthorAndFollowing(int page, int pageSize, List<String> followingIds)
        {
            return await _context.Cheeps
                .Include(c => c.Author)
                .Where(c => followingIds.Contains(c.Author.Id))
                .OrderByDescending(c => c.TimeStamp)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .Select(c => EntityToDTO.ToDTO(c))
                .ToListAsync();
        }

        public IQueryable<Cheep> GetCheepsFromAuthor()
        {
            return _context.Cheeps.Include(c => c.Author);
        }
        
        public async Task<List<CheepDTO>> GetPaginatedCheeps(int currentPage, int pageSize, IQueryable<Cheep> query)
        {
            return await query
                .OrderByDescending(c => c.TimeStamp)
                .Skip(pageSize * currentPage)
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
        
        public async Task<int> CountCheeps(List<string> followingIds)
        {
            return await _context.Cheeps
                .CountAsync(c => followingIds.Contains(c.Author.Id));
        }

        public IQueryable<Cheep> GetAllCheeps()
        {
            return _context.Cheeps;
        }
    }
}