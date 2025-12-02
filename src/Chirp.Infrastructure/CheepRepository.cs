using Chirp.Core.DomainModel;
using Chirp.Core.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chirp.Infrastructure
{
    public interface ICheepRepository
    {
        Task<List<CheepDTO>> GetCheeps(string? author = null);        
        Task<List<CheepDTO>> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null);        
        Task PostCheep(string text, string authorId);        
        Task<List<CheepDTO>> GetCheepsFromAuthorAndFollowing(int page, int pageSize, string authorName);
        Task<int> GetCheepCountFromAuthorAndFollowing(string authorName);
        Task<int> GetCheepCount(string? author = null);
        public Task LikePost(string currentUserId, int cheepIdToLike);
        public Task DislikePost(string currentUserId, int cheepIdToDislike);
        public Task RemoveDislike(string currentUserId, int cheepIdToUndislike);
        public Task RemoveLike(string currentUserId, int cheepIdToUnLike);
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

        public async Task<Author?> FindAuthor(string authorId)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.Id == authorId);
        }

        public async Task<Author?> FindAuthorAndCheeps(string authorName)
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

        public async Task LikePost(string currentUserId, int cheepIdToLike)
        {
            var cheepToLike = await _context.Cheeps
                .Include(c => c.Author)
                .Include(c => c.Likes)
                .FirstOrDefaultAsync(a => a.CheepId == cheepIdToLike);
            var userLiking = await _context.Authors
                .Include(a  => a.LikedCheeps)
                .FirstOrDefaultAsync(a => a.Id == currentUserId);

            if (cheepToLike == null || userLiking == null) return;
            if (cheepToLike.Author == userLiking) return;
            if (userLiking.LikedCheeps.Contains(cheepToLike) || cheepToLike.Likes.Contains(userLiking)) return;
            
            userLiking.LikedCheeps.Add(cheepToLike);
            cheepToLike.Likes.Add(userLiking);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveLike(string currentUserId, int cheepIdToUnlike)
        {
            var cheepToUnLike = await _context.Cheeps
                .Include(c => c.Likes)
                .FirstOrDefaultAsync(a => a.CheepId == cheepIdToUnlike);
            var userUnliking = await _context.Authors
                .Include(a  => a.LikedCheeps)
                .FirstOrDefaultAsync(a => a.Id == currentUserId);
            
            if (cheepToUnLike == null || userUnliking == null) return;
            if (!userUnliking.LikedCheeps.Contains(cheepToUnLike) || !cheepToUnLike.Likes.Contains(userUnliking)) return;
            
            userUnliking.LikedCheeps.Remove(cheepToUnLike);
            cheepToUnLike.Likes.Remove(userUnliking);
            await _context.SaveChangesAsync();
        }
        public async Task DislikePost(string currentUserId, int cheepIdToDislike)
        {
            
            var cheepToDislike = await _context.Cheeps
                .Include(c => c.Author)
                .Include(c => c.Dislikes)
                .FirstOrDefaultAsync(a => a.CheepId == cheepIdToDislike);
            var userDisliking = await _context.Authors
                .Include(a  => a.DislikedCheeps)
                .FirstOrDefaultAsync(a => a.Id == currentUserId);
            if (cheepToDislike == null || userDisliking == null) return;
            if (cheepToDislike.Author == userDisliking) return;
            if (userDisliking.DislikedCheeps.Contains(cheepToDislike) || cheepToDislike.Dislikes.Contains(userDisliking)) return;
            
            userDisliking.DislikedCheeps.Add(cheepToDislike);
            cheepToDislike.Dislikes.Add(userDisliking);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveDislike(string currentUserId, int cheepIdToUndislike)
        {
            var cheepToUndislike = await _context.Cheeps
                .Include(c => c.Dislikes)
                .FirstOrDefaultAsync(a => a.CheepId == cheepIdToUndislike);
            var userUndisliking = await _context.Authors
                .Include(a  => a.DislikedCheeps)
                .FirstOrDefaultAsync(a => a.Id == currentUserId);
            
            if (cheepToUndislike == null || userUndisliking == null) return;
            if (!cheepToUndislike.Dislikes.Contains(userUndisliking) || !userUndisliking.DislikedCheeps.Contains(cheepToUndislike)) return;
            
            userUndisliking.DislikedCheeps.Remove(cheepToUndislike);
            cheepToUndislike.Dislikes.Remove(userUndisliking);
            await _context.SaveChangesAsync();
        }
        
        public async Task<List<CheepDTO>> GetCheepsFromAuthorAndFollowing(int page, int pageSize, string authorName)
        {
            page--;
            var author = await _context.Authors
                .Include(a => a.Following)
                .FirstOrDefaultAsync(a => a.UserName == authorName);

            if (author == null) return new List<CheepDTO>();

            var followingIds = author.Following.Select(a => a.Id).ToList();
            followingIds.Add(author.Id);

            return await _context.Cheeps
                .Include(c => c.Author)
                .Where(c => followingIds.Contains(c.Author.Id))
                .OrderByDescending(c => c.TimeStamp)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .Select(c => EntityToDTO.ToDTO(c))
                .ToListAsync();
        }
        
        public async Task<List<CheepDTO>> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null)
        {
            currentPage--;
            IQueryable<Cheep> query = _context.Cheeps.Include(c => c.Author);

            if (!string.IsNullOrEmpty(author))
                query = query.Where(c => c.Author.UserName == author);

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
        
        public async Task<int> GetCheepCountFromAuthorAndFollowing(string authorName)
        {
            var author = await _context.Authors
                .Include(a => a.Following)
                .FirstOrDefaultAsync(a => a.UserName == authorName);

            if (author == null) return 0;

            var followingIds = author.Following.Select(a => a.Id).ToList();
            followingIds.Add(author.Id); 

            return await _context.Cheeps
                .CountAsync(c => followingIds.Contains(c.Author.Id));
        }
        
        public async Task<int> GetCheepCount(string? author = null)
        {
            IQueryable<Cheep> query = _context.Cheeps;
    
            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(c => c.Author.UserName == author);
            }
    
            return await query.CountAsync();
        }
    }
}