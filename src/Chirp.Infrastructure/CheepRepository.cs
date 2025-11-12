using Chirp.Core.DomainModel;
using Chirp.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chirp.Infrastructure
{
    public interface ICheepRepository
    {
        List<CheepDTO> GetCheeps(string? author = null);
        List<CheepDTO> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null);
        void PostCheep(string text, string authorName, string authorEmail); 
        void CreateUser(string authorName, string authorEmail);    }

    public class CheepRepository : ICheepRepository
    {
        private readonly ChirpDbContext _context;
        private readonly ILogger _logger;

        public CheepRepository(ChirpDbContext context, ILoggerFactory factory)
        {
            _context = context;
            _logger = factory.CreateLogger<CheepRepository>();
        }

        public List<CheepDTO> GetCheeps(string? author = null)
        {
            if (!string.IsNullOrEmpty(author))
            {
                var authorEntity = _context.Authors
                    .Include(a => a.Cheeps)
                    .FirstOrDefault(a => a.UserName == author);

                if (authorEntity == null)
                    return new List<CheepDTO>();

                return authorEntity.Cheeps
                    .OrderByDescending(c => c.TimeStamp)
                    .Select(c => EntityToDTO.ToDTO(c))
                    .ToList();
            }
            else
            {
                return _context.Cheeps
                    .Include(c => c.Author)
                    .OrderByDescending(c => c.TimeStamp)
                    .Select(c => EntityToDTO.ToDTO(c))
                    .ToList();
            }
        }

        public List<CheepDTO> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null)
        {
            IQueryable<Cheep> query = _context.Cheeps.Include(c => c.Author);

            if (!string.IsNullOrEmpty(author))
                query = query.Where(c => c.Author.UserName == author);

            return query
                .OrderByDescending(c => c.TimeStamp)
                .Skip(pageSize * currentPage)
                .Take(pageSize)
                .Select(c => EntityToDTO.ToDTO(c))
                .ToList();
        }

        public void PostCheep(string text, string authorName, string authorEmail)
        {
            CreateUser(authorName, authorEmail);

            var author = _context.Authors.FirstOrDefault(a => a.UserName == authorName);
            if (author == null)
            {
                _logger.LogWarning("No author found for user {User}", authorName);
                return;
            }

            var newCheep = new Cheep
            {
                Text = text,
                TimeStamp = DateTime.Now,
                //AuthorId = author.AuthorId,
                Author = author
            };

            _context.Cheeps.Add(newCheep);
            _context.SaveChanges();
        }

        public void CreateUser(string authorName, string authorEmail)
        {
            if (string.IsNullOrEmpty(authorName)) 
                return;
            
            if (_context.Authors.Any(a => a.UserName == authorName))
                return;

            var author = new Author
            {
                UserName = authorName,
                Email = authorEmail,
                Cheeps = new List<Cheep>()
            };

            _context.Authors.Add(author);
            _context.SaveChanges();
        }
    }
}