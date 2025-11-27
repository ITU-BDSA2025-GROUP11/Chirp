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
        private readonly ICheepRepository _service;
        private readonly UserManager<Author> _userManager;
        
        public List<string> Following { get; set; } = new();

        public List<CheepDTO> Cheeps { get; set; }  = new();
        public List<CheepDTO> CurrentPageCheeps { get; set; }  = new();
        public int NumberOfCheeps { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)NumberOfCheeps / PageSize);
        
        [BindProperty]
        public required string Message { get; set; } = "";
        public UserTimelineModel(ICheepRepository service, UserManager<Author> userManager)
        {
            _service = service;
            _userManager = userManager;
            NumberOfCheeps = Cheeps?.Count ?? 0;
        }

        public async Task<IActionResult> OnGet(string author, int? timelinepage)
        {
            if (_service.IsUserDeleted(author).Result) return NotFound();
            CurrentPage = timelinepage ?? 1;
            var ownTimeline = User.Identity?.IsAuthenticated == true && User.Identity?.Name == author;

            if (ownTimeline)
            {
                NumberOfCheeps = await _service.GetCheepCountFromAuthorAndFollowing(author); 
                CurrentPageCheeps = await _service.GetCheepsFromAuthorAndFollowing(CurrentPage, 32, author);
            }
            else
            {
                var allCheeps = await _service.GetCheeps(author); 
                NumberOfCheeps = allCheeps.Count;
        
                CurrentPageCheeps = await _service.GetPaginatedCheeps(CurrentPage - 1, 32, author);
            }

            if (User.Identity?.IsAuthenticated == true)
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId != null)
                {
                    ViewData["Following"] = await _service.GetFollowedIds(currentUserId);
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

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            await _service.PostCheep(Message, user.UserName ?? "unknown", user.Email ?? "unknown"); 

            return RedirectToPage(new { author });
        }
        
        public async Task<IActionResult> OnPostFollow(string authorId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                await _service.FollowUser(currentUserId, authorId);
            }
            var author = RouteData.Values["author"] as string; 
            return RedirectToPage(new { author });
        }

        public async Task<IActionResult> OnPostUnfollow(string authorId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                await _service.UnfollowUser(currentUserId, authorId);
            }
            var author = RouteData.Values["author"] as string;
            return RedirectToPage(new { author });
        }
    }
}