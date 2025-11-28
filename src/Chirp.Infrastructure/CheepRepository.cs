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
        Task PostCheep(string text, string authorName, string authorEmail);        
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
                query = query.Where(c => c.Author.UserName == author);

            return await query
                .OrderByDescending(c => c.TimeStamp)
                .Skip(pageSize * currentPage)
                .Take(pageSize)
                .Select(c => EntityToDTO.ToDTO(c))
                .ToListAsync(); 
        }

        public async Task PostCheep(string text, string authorName, string authorEmail)
        {
            if (text.Length > 160) throw new ArgumentOutOfRangeException("Your cheep is too long. Please keep it at 160 characters or less");
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.UserName == authorName);

            
            
            if (author == null)
            {
                throw new ArgumentException("Author not found");
            }
            
            var cheep = new Cheep
            {
                Text = text,
                Author = author,
                TimeStamp = DateTime.Now
            };
            _context.Cheeps.Add(cheep);
                    
            await _context.SaveChangesAsync();
            
            if (text.Length > 160)
            {
                _logger.LogWarning("{text} is longer than 160 chars", text);
            }
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
                .Where(c => followingIds.Contains(c.Author.Id))
                .OrderByDescending(c => c.TimeStamp)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .Select(c => EntityToDTO.ToDTO(c))
                .ToListAsync();
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