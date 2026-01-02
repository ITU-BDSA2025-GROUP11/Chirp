using Chirp.Core.DomainModel;
using Chirp.Core.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SQLitePCL;

namespace Chirp.Infrastructure;

public interface ICheepService
{
    /// <summary>
    /// ICheepService is used for dependency injection and describes the method signatures which the CheepService contains
    /// </summary>
    /// <param name="author"></param>
    /// <returns></returns>
    Task<List<CheepDTO>> GetCheeps(string? author = null);        
    Task<List<CheepDTO>> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null);        
    Task PostCheep(string text, string authorId);        
    Task<List<CheepDTO>> GetCheepsFromAuthorAndFollowing(string authorName);
    Task<List<CheepDTO>> GetPaginatedCheepsFromAuthorAndFollowing(int page, int pageSize, string authorName);
    Task<int> GetCheepCountFromAuthorAndFollowing(string authorName);
    Task<int> GetCheepCount(string? author = null);
    public Task LikePost(string currentUserId, int cheepIdToLike);
    public Task DislikePost(string currentUserId, int cheepIdToDislike);
    public Task RemoveDislike(string currentUserId, int cheepIdToUndislike);
    public Task RemoveLike(string currentUserId, int cheepIdToUnLike);
}
/// <summary>
/// CheepService is a class used by the application to interact with the database
/// it provides the ability to query, create, update and delete
/// </summary>
public class CheepService : ICheepService
{
    ICheepRepository _cheepRepository;
    ILogger<CheepService> _logger;

    public CheepService(ICheepRepository cheepRepository, ILogger<CheepService> logger)
    {
        _cheepRepository = cheepRepository;
        _logger = logger;
    }
    /// <summary>
    /// Get all cheeps in the database
    /// if an author is provided all cheeps written by that author are retrieved and only cheeps by that author
    /// </summary>
    /// <param name="author">optional specification</param>
    /// <returns></returns>
    public async Task<List<CheepDTO>> GetCheeps(string? author = null)
    {
        {
            if (!string.IsNullOrEmpty(author))
            {
                var authorEntity = await _cheepRepository.GetAuthorAndCheeps(author);

                if (authorEntity == null)
                    return new List<CheepDTO>();

                return authorEntity.Cheeps
                    .OrderByDescending(c => c.TimeStamp)
                    .Select(c => EntityToDTO.ToDTO(c))
                    .ToList();
            }

            return await _cheepRepository.GetCheeps();

        }
    }        
    /// <summary>
    /// Method for posting a cheep, and adding it to the database
    /// </summary>
    /// <param name="text">the cheep text</param>
    /// <param name="authorId">id of author posting cheep</param>
    /// <exception cref="ArgumentException">throws an exception if non-existing author is trying to post a cheep</exception>
    public async Task PostCheep(string text, string authorId)
    {
        if (text.Length > 160) return;
        var author = await _cheepRepository.GetAuthorAndCheepsFromId(authorId);
        if (author == null) throw new ArgumentException("Author not found");
        var cheep = new Cheep
        {
            Text = text,
            Author = author,
            TimeStamp = DateTime.Now
        };
        author.Cheeps.Add(cheep);
        await _cheepRepository.SaveChanges();
    }      
    
    public async Task<bool> IsFollowing(string currentUserId, string authorId)
    {
        return await _cheepRepository.IsFollowing(currentUserId, authorId);
    }
    /// <summary>
    /// method for liking a cheep
    /// </summary>
    /// <param name="currentUserId">user who is liking</param>
    /// <param name="cheepIdToLike">cheep being liked</param>
    public async Task LikePost(string currentUserId, int cheepIdToLike)
    {
        var cheepToLike = await _cheepRepository.GetAuthorCheepAndLikes(cheepIdToLike);

        var userLiking = await _cheepRepository.GetAuthorAndLikedCheeps(currentUserId);

        if (cheepToLike == null || userLiking == null) return;
        if (cheepToLike.Author == userLiking) return;
        if (userLiking.LikedCheeps.Contains(cheepToLike) || cheepToLike.Likes.Contains(userLiking)) return;
        
        userLiking.LikedCheeps.Add(cheepToLike);
        cheepToLike.Likes.Add(userLiking);
        await _cheepRepository.SaveChanges();
    }
    /// <summary>
    /// method for removing a like
    /// </summary>
    /// <param name="currentUserId">user unliking</param>
    /// <param name="cheepIdToUnLike">cheep to unlike</param>
    public async Task RemoveLike(string currentUserId, int cheepIdToUnLike)
    {
        var cheepToUnLike = await _cheepRepository.GetAuthorCheepAndLikes(cheepIdToUnLike);
        
        var userUnliking = await _cheepRepository.GetAuthorAndLikedCheeps(currentUserId);
            
        if (cheepToUnLike == null || userUnliking == null) return;
        if (!userUnliking.LikedCheeps.Contains(cheepToUnLike) || !cheepToUnLike.Likes.Contains(userUnliking)) return;
            
        userUnliking.LikedCheeps.Remove(cheepToUnLike);
        cheepToUnLike.Likes.Remove(userUnliking);
        await _cheepRepository.SaveChanges();
    }
    /// <summary>
    /// method for disliking a post
    /// </summary>
    /// <param name="currentUserId">user disliking</param>
    /// <param name="cheepIdToDislike">cheep to dislike</param>
    public async Task DislikePost(string currentUserId, int cheepIdToDislike)
    {
        var cheepToDislike = await _cheepRepository.GetAuthorCheepAndDislikes(cheepIdToDislike);
        var userDisliking = await _cheepRepository.GetAuthorAndDislikedCheeps(currentUserId);
        
        if (cheepToDislike == null || userDisliking == null) return;
        if (cheepToDislike.Author == userDisliking) return;
        if (userDisliking.DislikedCheeps.Contains(cheepToDislike) || cheepToDislike.Dislikes.Contains(userDisliking)) return;
            
        userDisliking.DislikedCheeps.Add(cheepToDislike);
        cheepToDislike.Dislikes.Add(userDisliking);
        await _cheepRepository.SaveChanges();
    }
    /// <summary>
    /// method for removing a dislike
    /// </summary>
    /// <param name="currentUserId">user undisliking</param>
    /// <param name="cheepIdToUndislike">cheep to undislike</param>
    public async Task RemoveDislike(string currentUserId, int cheepIdToUndislike)
    {
        var cheepToUndislike = await _cheepRepository.GetAuthorCheepAndDislikes(cheepIdToUndislike);
        var userUndisliking = await _cheepRepository.GetAuthorAndDislikedCheeps(currentUserId);
            
        if (cheepToUndislike == null || userUndisliking == null) return;
        if (!cheepToUndislike.Dislikes.Contains(userUndisliking) || !userUndisliking.DislikedCheeps.Contains(cheepToUndislike)) return;
            
        userUndisliking.DislikedCheeps.Remove(cheepToUndislike);
        cheepToUndislike.Dislikes.Remove(userUndisliking);
        await _cheepRepository.SaveChanges();
    }
    /// <summary>
    /// get all cheeps to be shown on a users own timeline on a single page (usually of length 32)
    /// all cheeps written by the user and all cheeps from users followed by the logged in user
    /// </summary>
    /// <param name="page">current page</param>
    /// <param name="pageSize">size of the page</param>
    /// <param name="authorName">name of logged-in user</param>
    /// <returns></returns>
    public async Task<List<CheepDTO>> GetPaginatedCheepsFromAuthorAndFollowing(int page, int pageSize, string authorName)
    {
        var author = await _cheepRepository.GetAuthorNameAndFollowing(authorName);

        if (author == null) return new List<CheepDTO>();

        var followingIds = author.Following.Select(a => a.Id).ToList();
        followingIds.Add(author.Id);

        return await _cheepRepository.GetPaginatedCheepsFromAuthorAndFollowing(page, pageSize, followingIds);
    }
    /// <summary>
    /// get all cheeps from logged-in user and all cheeps from all users followed by logged-in user
    /// </summary>
    /// <param name="authorName">logged-in user</param>
    /// <returns></returns>
    public async Task<List<CheepDTO>> GetCheepsFromAuthorAndFollowing(string authorName)
    {
        var author = await _cheepRepository.GetAuthorNameAndFollowing(authorName);

        if (author == null) return new List<CheepDTO>();

        var followingIds = author.Following.Select(a => a.Id).ToList();
        followingIds.Add(author.Id);

        return await _cheepRepository.GetCheepsFromAuthorAndFollowing(followingIds);
    }
    /// <summary>
    /// method for getting only the current page cheeps on the public timeline
    /// </summary>
    /// <param name="currentPage"></param>
    /// <param name="pageSize"></param>
    /// <param name="author">optional parameter</param>
    /// <returns></returns>
    public Task<List<CheepDTO>> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null)
    {
        currentPage--;
        IQueryable<Cheep> query = _cheepRepository.GetCheepsFromAuthor();

        if (!string.IsNullOrEmpty(author))
            query = query.Where(c => c.Author.UserName == author);

        return _cheepRepository.GetPaginatedCheeps(currentPage, pageSize, query);
    }
    /// <summary>
    /// get the number of cheeps from a logged-in user summed with the count of all cheeps
    /// from all users followed by logged-in user
    /// used for calculating how many pages there are in total, shown on the bottom of all pages
    /// </summary>
    /// <param name="authorName">logged-in user</param>
    /// <returns></returns>
    public async Task<int> GetCheepCountFromAuthorAndFollowing(string authorName)
    {
        var author = await _cheepRepository.GetAuthorNameAndFollowing(authorName);

        if (author == null) return 0;

        var followingIds = author.Following.Select(a => a.Id).ToList();
        followingIds.Add(author.Id); 

        return await _cheepRepository.CountCheeps(followingIds);
    }
    /// <summary>
    /// method for getting the count of all cheeps in the entire database
    /// used to calculate the number of pages on the public timeline
    /// </summary>
    /// <param name="author">optional param</param>
    /// <returns></returns>
    public async Task<int> GetCheepCount(string? author = null)
    {
        IQueryable<Cheep> query = _cheepRepository.GetAllCheeps();
    
        if (!string.IsNullOrEmpty(author))
        {
            query = query.Where(c => c.Author.UserName == author);
        }
    
        return await query.CountAsync();
    }
}