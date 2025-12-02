using Chirp.Core.DTO;
using Microsoft.Extensions.Logging;

namespace Chirp.Infrastructure;

public interface IAuthorService
{
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

    Task<List<string>> GetFollowedIds(string userId)
    {
        
    }
    Task FollowUser(string currentUserId, string authorIdToFollow);
    Task UnfollowUser(string currentUserId, string authorIdToUnfollow);
    Task<bool> IsFollowing(string currentUserId, string authorId);  
    Task<bool> DeleteUser(string username);
    Task<bool> IsUserDeleted(string username);
    public Task<List<int>> GetLikedCheepIds(string userId);
    public Task<List<int>> GetDislikedCheepIds(string userId);
}
