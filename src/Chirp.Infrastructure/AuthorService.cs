using Chirp.Core.DomainModel;
using Chirp.Core.DTO;
using Microsoft.Extensions.Logging;
using SQLitePCL;

namespace Chirp.Infrastructure;

public interface IAuthorService
{
    Task CreateUser(string authorName, string authorEmail);
    Task<List<string>> GetFollowedIds(string userId);
    Task FollowUser(string currentUserId, string authorIdToFollow);
    Task UnfollowUser(string currentUserId, string authorIdToUnfollow);
    Task<bool> IsFollowing(string currentUserId, string authorId);  
    Task<bool> DeleteUser(string username);
    Task<bool> IsUserDeleted(string username);
    public Task<List<int>> GetLikedCheepIds(string userId);
    public Task<List<int>> GetDislikedCheepIds(string userId);
}

public class AuthorService : IAuthorService
{
    private readonly AuthorRepository _authorRepository;
    private readonly ILogger<AuthorService> _logger;


    public AuthorService(AuthorRepository authorRepository, ILogger<AuthorService> logger)
    {
        _authorRepository = authorRepository;
        _logger = logger;
    }

    public async Task CreateUser(string authorName, string authorEmail)
    {
        if (string.IsNullOrEmpty(authorName))
            return;

        if (await _authorRepository.UserExists(authorName)) 
            return;
        
        var author = new Author
        {
            UserName = authorName,
            Email = authorEmail,
            Cheeps = new List<Cheep>()
        };

        await _authorRepository.AddUser(author);
    }
    
    Task<List<string>> GetFollowedIds(string userId)
    {
        var user = await _context.Authors
            .Include(a => a.Following)
            .FirstOrDefaultAsync(a => a.Id == userId);

        if (user == null) return new List<string>();

        return user.Following.Select(a => a.Id).ToList();
    }

    Task FollowUser(string currentUserId, string authorIdToFollow)
    {
        
    }

    Task UnfollowUser(string currentUserId, string authorIdToUnfollow)
    {
        
    }

    Task<bool> IsFollowing(string currentUserId, string authorId)
    {
        
    }

    Task<bool> DeleteUser(string username)
    {
        
    }

    Task<bool> IsUserDeleted(string username)
    {
        return await GetUserInfo(username) == null;
    }

    public Task<List<int>> GetLikedCheepIds(string userId)
    {
        
    }

    public Task<List<int>> GetDislikedCheepIds(string userId)
    {
        
    }
}
