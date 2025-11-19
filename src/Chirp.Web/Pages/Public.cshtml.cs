using Chirp.Core.DTO;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Web.Pages
{
    public class PublicModel : PaginationModel
    {
        private readonly ICheepRepository _service;
        private readonly UserManager<Author> _userManager;
        
        public List<CheepDTO> CurrentPageCheeps { get; set; } = new();
        
        public List<string> Following { get; set; } = new();
        
        public List<CheepDTO> Cheeps { get; set; } = new();

        public int NumberOfCheeps { get; set; }
        
        public int TotalPages => GetTotalPages(NumberOfCheeps, PageSize);

        [BindProperty] public required string Message { get; set; }

        public PublicModel(ICheepRepository service, UserManager<Author> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(int? publicpage = 1)
        {
            CurrentPage = publicpage ?? 1;

            Cheeps = await _service.GetCheeps();
    
            NumberOfCheeps = await _service.GetCheepCount(null);
            
            CurrentPageCheeps = await _service.GetPaginatedCheeps(CurrentPage - 1, PageSize);
            
            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewData["Following"] = await _service.GetFollowedIds(currentUserId);
            }
    
            return Page();
        }
        
        public async Task<IActionResult> OnPostFollow(string authorId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                await _service.FollowUser(currentUserId, authorId);
            }
            
            return RedirectToPage(); 
        }
        
        public async Task<IActionResult> OnPostUnfollow(string authorId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                await _service.UnfollowUser(currentUserId, authorId);
            }
        
            return RedirectToPage();
        }
        
        private int GetTotalPages(int numberOfCheeps, int pageSize)
        {
            return (int)Math.Ceiling((double)numberOfCheeps / pageSize);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Message))
            {
                return RedirectToPage();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            await _service.PostCheep(Message, user.UserName, user.Email);
            return RedirectToPage();
        }
    }
}