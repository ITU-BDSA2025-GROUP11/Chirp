using Chirp.Core.DTO;
using System.Security.Claims;
using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;


namespace Chirp.Web.Pages
{
    public class UserTimelineModel : PaginationModel
    {
        private readonly IAuthorService _authorService;
        private readonly ICheepService _cheepService;
        private readonly UserManager<Author> _userManager;
        
        public List<string> Following { get; set; } = new();
        public List<CheepDTO> Cheeps { get; set; }  = new();
        public List<CheepDTO> CurrentPageCheeps { get; set; }  = new();
        public int NumberOfCheeps { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)NumberOfCheeps / PageSize);
        
        [BindProperty]
        public required string Message { get; set; } = "";
        public UserTimelineModel(ICheepService cheepService,IAuthorService authorService, UserManager<Author> userManager)
        {
            _authorService = authorService;
            _cheepService = cheepService;
            _userManager = userManager;
            NumberOfCheeps = Cheeps?.Count ?? 0;
        }

        public async Task<IActionResult> OnGet(string author, int? timelinepage)
        {
            if (_authorService.IsUserDeleted(author).Result) return NotFound();
            Cheeps =  await _cheepService.GetCheeps(author);
            CurrentPage = timelinepage ?? 1;
            var ownTimeline = User.Identity?.IsAuthenticated == true && User.Identity?.Name == author;
            if (ownTimeline)
            {
                NumberOfCheeps = await _cheepService.GetCheepCountFromAuthorAndFollowing(author); 
                CurrentPageCheeps = await _cheepService.GetCheepsFromAuthorAndFollowing(CurrentPage, 32, author);
            }
            else
            {
                var allCheeps = await _cheepService.GetCheeps(author); 
                NumberOfCheeps = allCheeps.Count;
        
                CurrentPageCheeps = await _cheepService.GetPaginatedCheeps(CurrentPage, 32, author);
            }

            if (User.Identity?.IsAuthenticated == true)
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId != null)
                {
                    ViewData["Following"] = await _authorService.GetFollowedIds(currentUserId);
                    ViewData["LikedCheeps"] = await _authorService.GetLikedCheepIds(currentUserId);
                    ViewData["DislikedCheeps"] = await _authorService.GetDislikedCheepIds(currentUserId);
                }
            }

            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync(string author)
        {
            if (string.IsNullOrEmpty(Message))
            {
                return RedirectToPage(new { author });
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Challenge();
            }

            await _cheepService.PostCheep(Message, currentUserId); 

            return RedirectToPage(new { author });
        }
        
        public async Task<IActionResult> OnPostFollow(string authorId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                await _authorService.FollowUser(currentUserId, authorId);
            }
            var author = RouteData.Values["author"] as string; 
            return RedirectToPage(new { author });
        }

        public async Task<IActionResult> OnPostUnfollow(string authorId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                await _authorService.UnfollowUser(currentUserId, authorId);
            }
            var author = RouteData.Values["author"] as string;
            return RedirectToPage(new { author });
        }
        public async Task<IActionResult> OnPostLike(int cheepId)
        {
            Console.WriteLine("I AM LIKING CHEEP: " + cheepId);
            //var cheepid =  
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                await _cheepService.LikePost(currentUserId, cheepId);
            }
        
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostUnlike(int cheepId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                await _cheepService.RemoveLike(currentUserId, cheepId);
            }
        
            return RedirectToPage();
        }
        
        public async Task<IActionResult> OnPostDislike(int cheepId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                await _cheepService.DislikePost(currentUserId, cheepId);
            }
        
            return RedirectToPage();
        }
        public int GetTotalPages(int numberOfCheeps, int pageSize)
        {
            return (int)Math.Ceiling((double)numberOfCheeps / pageSize);
        }
    }
}