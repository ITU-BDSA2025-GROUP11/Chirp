using Chirp.Core.DTO;
using Microsoft.Extensions.Logging;

namespace Chirp.Infrastructure;

public interface ICheepService
{
    Task<List<CheepDTO>> GetPaginatedCheeps(int currentPage, int pageSize, string? author = null);  
    Task<List<CheepDTO>> GetCheepsFromAuthorAndFollowing(int page, int pageSize, string authorName);
    Task<int> GetCheepCountFromAuthorAndFollowing(string authorName);
    Task<int> GetCheepCount(string? author = null);
    public Task LikePost(string currentUserId, int cheepIdToLike);
    public Task DislikePost(string currentUserId, int cheepIdToDislike);
    public Task RemoveDislike(string currentUserId, int cheepIdToUndislike);
    public Task RemoveLike(string currentUserId, int cheepIdToUnLike);
}

public class CheepService
{
    CheepRepository _cheepRepository;
    ILogger<CheepService> _logger;
}