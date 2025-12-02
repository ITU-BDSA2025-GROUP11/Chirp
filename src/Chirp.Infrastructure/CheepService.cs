using Chirp.Core.DomainModel;
using Chirp.Core.DTO;
using Microsoft.Extensions.Logging;

namespace Chirp.Infrastructure;

public interface ICheepService
{
    Task<List<CheepDTO>> GetCheeps(string? author = null);        
    Task<List<CheepDTO>> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null);        
    Task PostCheep(string text, string authorId);        
    Task<List<CheepDTO>> GetCheepsFromAuthorAndFollowing(int page, int pageSize, string authorName);
    Task<int> GetCheepCountFromAuthorAndFollowing(string authorName);
    Task<int> GetCheepCount(string? author = null);
    public Task LikePost(string currentUserId, int cheepIdToLike);
    public Task DislikePost(string currentUserId, int cheepIdToDislike);
    public Task RemoveDislike(string currentUserId, int cheepIdToUndislike);
    public Task RemoveLike(string currentUserId, int cheepIdToUnLike);
}

public class CheepService : ICheepService
{
    CheepRepository _cheepRepository;
    ILogger<CheepService> _logger;

    public CheepService(CheepRepository cheepRepository, ILogger<CheepService> logger)
    {
        _cheepRepository = cheepRepository;
        _logger = logger;
    }

    public async Task<List<CheepDTO>> GetCheeps(string? author = null)
    {
        {
            if (!string.IsNullOrEmpty(author))
            {
                var authorEntity = await _cheepRepository.FindAuthorAndCheeps(author);

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
    Task<List<CheepDTO>> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null);

    public async Task PostCheep(string text, string authorId)
    {
        if (text.Length > 160) return; //throw new ArgumentException("Your cheep is too long. Please keep it at 160 characters or less");
        var author = await _cheepRepository.FindAuthor(authorId);
        if (author == null) throw new ArgumentException("Author not found");
        var cheep = new Cheep
        {
            Text = text,
            Author = author,
            TimeStamp = DateTime.Now
        };
        _cheepRepository.AddCheep(cheep);
        author.Cheeps.Add(cheep);
        await _cheepRepository.SaveChanges();
    }      
    Task<List<CheepDTO>> GetCheepsFromAuthorAndFollowing(int page, int pageSize, string authorName);
    Task<int> GetCheepCountFromAuthorAndFollowing(string authorName);
    Task<int> GetCheepCount(string? author = null);
    public Task LikePost(string currentUserId, int cheepIdToLike);
    public Task DislikePost(string currentUserId, int cheepIdToDislike);
    public Task RemoveDislike(string currentUserId, int cheepIdToUndislike);
    public Task RemoveLike(string currentUserId, int cheepIdToUnLike);
    
    
}