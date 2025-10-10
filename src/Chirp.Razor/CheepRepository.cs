using Chirp.Razor.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;

public interface ICheepRepository
{
    public List<Cheep> GetCheeps(string? author = null);
    public List<Cheep> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null);

    public void PostCheep(String text);
}

public class CheepRepository : ICheepRepository
{
    readonly ChirpDbContext _context;
    readonly ILogger _logger;

    public CheepRepository(ChirpDbContext context, ILoggerFactory factory)
    {
        _context = context;
        _logger = factory.CreateLogger<CheepRepository>();
    }

    public List<Cheep> GetCheeps(string? author = null)
    {

        if (author != null)
        {
            var _ = _context.Authors
                .Include(a => a.Cheeps)
                .FirstOrDefault(a => a.Username == author);

            return _.Cheeps.ToList();
        }
        else
        {
            var _ = _context.Cheeps.ToList();
            return _;
        }
    }

    public List<Cheep> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null)
    {
        if (!string.IsNullOrEmpty(author))
        {
            return _context.Cheeps
                .OrderByDescending(c => c.TimeStamp)
                .Where(c => c.Author.Username == author)
                .Skip(pageSize * currentPage)
                .Take(pageSize)
                .ToList();
        }
        else
        {
            return _context.Cheeps
                .OrderByDescending(c => c.TimeStamp)
                .Skip(pageSize * currentPage)
                .Take(pageSize)
                .ToList();
        }
    }

    public void PostCheep(String text)
    {
        //_context.Cheeps.Add(new Cheep { Text = text, TimeStamp = DateTime.Now, Author = Environment.UserName });
        _context.SaveChanges();
    }
}