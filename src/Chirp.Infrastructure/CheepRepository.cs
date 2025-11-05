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
        void PostCheep(string text);
        void CreateUser();
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

        public List<CheepDTO> GetCheeps(string? author = null)
        {
            if (!string.IsNullOrEmpty(author))
            {
                var authorEntity = _context.Users
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

        public void PostCheep(string text)
        {
            CreateUser();

            var author = _context.Users.FirstOrDefault(a => a.UserName == Environment.UserName);
            if (author == null)
            {
                _logger.LogWarning("No author found for user {User}", Environment.UserName);
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

        public void CreateUser()
        {
            var name = Environment.UserName; // ?? "Anonymous";

            if (_context.Users.Any(a => a.UserName == name))
                return;

            var author = new Author
            {
                UserName = name,
                Email = $"{name}@mail.com",
                Cheeps = new List<Cheep>()
            };

            _context.Users.Add(author);
            _context.SaveChanges();
        }
    }
}
