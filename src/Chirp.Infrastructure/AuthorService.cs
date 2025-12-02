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
    
    public async Task<List<string>> GetFollowedIds(string userId)
    {
        var user = await _authorRepository.FindUserAndFollowing(userId);
        
        if (user == null) return new List<string>();

        return _authorRepository.GetFollowedIds(user);
    }

    public async Task FollowUser(string currentUserId, string authorIdToFollow)
    {
        var userToFollow = await _authorRepository.FindUser(authorIdToFollow);

        var currentUser = await _authorRepository.FindUserAndFollowing(currentUserId);

        if (userToFollow == null || currentUser == null) return;

        if (!currentUser.Following.Contains(userToFollow))
        {
            currentUser.Following.Add(userToFollow);
            await _authorRepository.SaveChanges();
        }
    }

    public async Task UnfollowUser(string currentUserId, string authorIdToUnfollow)
    {
        var userToUnfollow = await _authorRepository.FindUser(authorIdToUnfollow);

        var currentUser = await _authorRepository.FindUserAndFollowing(currentUserId);
        
        if (userToUnfollow == null || currentUser == null) return;

        currentUser.Following.Remove(userToUnfollow);
        await _authorRepository.SaveChanges();
    }
    
    public async Task<UserInfoDTO?> GetUserInfo(string username)
    {
        var author = await _authorRepository.GetUserInfo(username);

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

    Task<bool> DeleteUser(string username)
    {
        
    }

    public async Task<bool> IsUserDeleted(string username)
    {
        return await IsUserDeleted(username);
    }
    
    public Task<List<int>> GetLikedCheepIds(string userId)
    {
        
    }

    public Task<List<int>> GetDislikedCheepIds(string userId)
    {
        
    }

    
}
