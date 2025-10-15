using Chirp.Razor.DomainModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;

public interface ICheepRepository
{
    
    // states that every method should either be a command that performs an action,
    // or a query that returns data to the caller, but not both.
    
    // so were not allowed  to add a new Cheep or updating/deleting, in same method that queries
    
    public List<Cheep> GetCheeps(string? author = null);
    public List<Cheep> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null);

    public void PostCheep(String text, string? author = null);

    public Author? GetAuthor(String text);

    public void CreateAuthor(String? text = null);

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

    public void PostCheep(String text, string? author = null)
    {
        string validAuthor;
        
        if (author == null || GetAuthor(author) == null || GetAuthor(Environment.UserName) == null )
        {
            Console.WriteLine("AUTHOR WAS NOT FOUND \n Created New author: " + Environment.UserName);
            CreateAuthor();
            validAuthor = Environment.UserName;
        }
        else
        {
            validAuthor = author;
        }

        var cheep = new Cheep
        {
            Text = text,
            TimeStamp = DateTime.Now,
            Author = GetAuthor(validAuthor)
            };
        
        _context.Cheeps.Add(cheep);
        _context.SaveChanges();
    }

    public void CreateAuthor(String? text = null)
    {
        Author author;
        
        if (text != null)
        {
             author = new Author
            {
                Username = text,
                Email = text + "@email.com",
                Cheeps = new List<Cheep>()
            };
        }
        else
        {
            author = new Author
            {
                Username = Environment.UserName,
                Email = Environment.UserName + "@email.com",
                Cheeps = new List<Cheep>()
            };
        }
        
        _context.Authors.Add(author);
        _context.SaveChanges();
    }

    public Author? GetAuthor(String text)
    {
        if (text.Contains("@"))
        {
            return _context.Authors
                .FirstOrDefault(a => a.Email == text);
        }
        
        return _context.Authors
            .FirstOrDefault(a => a.Username == text);
    }
}