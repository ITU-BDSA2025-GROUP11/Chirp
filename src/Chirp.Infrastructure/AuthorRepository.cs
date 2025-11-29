using Chirp.Core.DomainModel;
using Chirp.Core.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chirp.Infrastructure;

public interface IAuthorRepository
{
    Task CreateUser(string authorName, string authorEmail);
    Task<List<string>> GetFollowedIds(string userId);
    Task FollowUser(string currentUserId, string authorIdToFollow);
    Task UnfollowUser(string currentUserId, string authorIdToUnfollow);
    Task<bool> IsFollowing(string currentUserId, string authorId);  
    Task<UserInfoDTO?> GetUserInfo(string username);
    Task<bool> DeleteUser(string username);
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
    
    public async Task<List<string>> GetFollowedIds(string userId)
    {
        var user = await _context.Authors
            .Include(a => a.Following)
            .FirstOrDefaultAsync(a => a.Id == userId);

        if (user == null) return new List<string>();

        return user.Following.Select(a => a.Id).ToList();
    }

    public async Task<UserInfoDTO?> GetUserInfo(string username)
    {
        var author = await _context.Authors
            .Where(a => a.UserName == username)
            .Include(a => a.Cheeps)
            .Include(a => a.Following)
            .FirstOrDefaultAsync();

        if (author == null) return null;

        return new UserInfoDTO
        {
            Name = author.UserName ?? string.Empty,
            Email = author.Email ?? string.Empty,
                
            Cheeps = author.Cheeps
                .OrderByDescending(c => c.TimeStamp)
                .Select(c => EntityToDTO.ToDTO(c))
                .ToList(),
            FollowedUsernames = author.Following.Select(f => f.UserName).ToList()!
        };
    }

    public async Task<bool> IsUserDeleted(string username)
    {
        return await GetUserInfo(username) == null;
    }

    public async Task<bool> DeleteUser(string username)
    {
        var author = await _context.Authors
            .Include(a => a.Following)
            .Include(a => a.Followers)
            .Include(a => a.Cheeps)
            .FirstOrDefaultAsync(a => a.UserName == username);

        if (author == null) return false;
        
        author.Following.Clear();
        author.Followers.Clear();

        var uniqueId = Guid.NewGuid().ToString().Substring(0, 8);
        author.UserName = $"DeletedUser-{uniqueId}";
        author.NormalizedUserName = $"DELETEDUSER-{uniqueId}";
        author.Email = $"deleted-{uniqueId}@chirp.db";
        author.NormalizedEmail = $"DELETED-{uniqueId}@CHIRP.DB";
        
        await _context.SaveChangesAsync();
        return true;
    }
}