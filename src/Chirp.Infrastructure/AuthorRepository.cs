using Chirp.Core.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chirp.Infrastructure;

public interface IAuthorRepository
{
    Task AddUser(Author author);
    Task<bool> UserExists(string authorName);
    Task SaveChanges();
    Task<Author?> FindUser(string authorId);
    Task<Author?> FindUserAndFollowing(string authorId);
    Task<Author?> FindUserAndLikedCheeps(string authorId);
    Task<bool> IsFollowing(string currentUserId, string authorId);
    Task<List<int>> GetLikedCheepIds(string userId);
    Task<List<int>> GetDislikedCheepIds(string userId);
    Task<Author?> GetUserInfo(string username);
    Task<Author?> GetAllUserInfo(string username);
    Task<bool> IsUserDeleted(string username);
}

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpDbContext _context;
    private readonly ILogger<AuthorRepository> _logger;
    
    public AuthorRepository(ChirpDbContext context, ILoggerFactory factory)
    {
        _context = context;
        _logger = factory.CreateLogger<AuthorRepository>();
    }
    
    public async Task AddUser(Author author)
    {
        _context.Authors.Add(author);
        await SaveChanges();
    }

    public async Task<bool> UserExists(string authorName)
    {
        return await _context.Authors.AnyAsync(a => a.UserName == authorName);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Author?> FindUser(string authorId)
    {
        return await _context.Authors.FindAsync(authorId);
    }

    public async Task<Author?> FindUserAndFollowing(string authorId)
    {
        return await _context.Authors
            .Include(a => a.Following)
            .FirstOrDefaultAsync(a => a.Id == authorId);
    }

    public async Task<Author?> FindUserAndLikedCheeps(string authorId)
    {
        return await _context.Authors
            .Include(a => a.LikedCheeps)
            .FirstOrDefaultAsync(a => a.Id == authorId);
    }

    public async Task<Author?> FindUserAndDislikedCheeps(string authorId)
    {
        return await _context.Authors
            .Include(a => a.DislikedCheeps)
            .FirstOrDefaultAsync(a => a.Id == authorId);
    }
    
    public async Task<bool> IsFollowing(string currentUserId, string authorId)
    {
        return await _context.Authors
            .AnyAsync(a => a.Id == currentUserId && a.Following.Any(f => f.Id == authorId));
    }

    public async Task<Author?> GetUserInfo(string username)
    {
        return await _context.Authors
            .Where(a => a.UserName == username)
            .Include(a => a.Cheeps)
            .Include(a => a.Following)
            .FirstOrDefaultAsync();
    }

    public async Task<Author?> GetAllUserInfo(string username)
    {
        return await _context.Authors
            .Include(a => a.Following)
            .Include(a => a.Followers)
            .Include(a => a.Cheeps)
            .FirstOrDefaultAsync(a => a.UserName == username);
    }
    
    public async Task<List<int>> GetLikedCheepIds(string userId)
    {
        var user = await _context.Authors
            .Include(a => a.LikedCheeps)
            .FirstOrDefaultAsync(a => a.Id == userId);
        
        if (user == null) return new List<int>();
        
        return user.LikedCheeps.Select(c => c.CheepId).ToList();
    }
    public async Task<List<int>> GetDislikedCheepIds(string userId)
    {
        var user = await _context.Authors
            .Include(a => a.DislikedCheeps)
            .FirstOrDefaultAsync(a => a.Id == userId);
        
        if (user == null) return new List<int>();
        
        return user.DislikedCheeps.Select(a => a.CheepId).ToList();
    }

    public async Task<bool> IsUserDeleted(string username)
    {
        return await GetUserInfo(username) == null;
    }
}