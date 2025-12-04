using Chirp.Core.DomainModel;
using Chirp.Core.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SQLitePCL;

namespace Chirp.Infrastructure;

public interface ICheepService
{
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

public class CheepService : ICheepService
{
    ICheepRepository _cheepRepository;
    ILogger<CheepService> _logger;

    public CheepService(ICheepRepository cheepRepository, ILogger<CheepService> logger)
    {
        _cheepRepository = cheepRepository;
        _logger = logger;
    }

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

    public async Task PostCheep(string text, string authorId)
    {
        if (text.Length > 160) return; //throw new ArgumentException("Your cheep is too long. Please keep it at 160 characters or less");
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

    public async Task<List<CheepDTO>> GetPaginatedCheepsFromAuthorAndFollowing(int page, int pageSize, string authorName)
    {
        var author = await _cheepRepository.GetAuthorNameAndFollowing(authorName);

        if (author == null) return new List<CheepDTO>();

        var followingIds = author.Following.Select(a => a.Id).ToList();
        followingIds.Add(author.Id);

        return await _cheepRepository.GetPaginatedCheepsFromAuthorAndFollowing(page, pageSize, followingIds);
    }

    public async Task<List<CheepDTO>> GetCheepsFromAuthorAndFollowing(string authorName)
    {
        var author = await _cheepRepository.GetAuthorNameAndFollowing(authorName);

        if (author == null) return new List<CheepDTO>();

        var followingIds = author.Following.Select(a => a.Id).ToList();
        followingIds.Add(author.Id);

        return await _cheepRepository.GetCheepsFromAuthorAndFollowing(followingIds);
    }

    public Task<List<CheepDTO>> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null)
    {
        currentPage--;
        IQueryable<Cheep> query = _cheepRepository.GetCheepsFromAuthor();

        if (!string.IsNullOrEmpty(author))
            query = query.Where(c => c.Author.UserName == author);

        return _cheepRepository.GetPaginatedCheeps(currentPage, pageSize, query);
    }
    
    // public async Task<List<string>> GetFollowedIds(string userId)
    // {
    //     var user = await _cheepRepository.(userId);
    //
    //     if (user == null) return new List<string>();
    //
    //     return user.Following.Select(a => a.Id).ToList();
    // }

    public async Task<int> GetCheepCountFromAuthorAndFollowing(string authorName)
    {
        var author = await _cheepRepository.GetAuthorNameAndFollowing(authorName);

        if (author == null) return 0;

        var followingIds = author.Following.Select(a => a.Id).ToList();
        followingIds.Add(author.Id); 

        return await _cheepRepository.CountCheeps(followingIds);
    }

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