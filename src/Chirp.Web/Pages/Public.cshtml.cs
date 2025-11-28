using System.Security.Claims;
using Chirp.Core.DTO;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Web.Pages
{
    public class PublicModel : PaginationModel
    {
        private readonly IAuthorRepository _authorService;
        private readonly ICheepRepository _cheepService;
        private readonly UserManager<Author> _userManager;
        
        public List<CheepDTO> CurrentPageCheeps { get; set; } = new();
        
        public List<string> Following { get; set; } = new();
        
        public List<CheepDTO> Cheeps { get; set; } = new();

        public int NumberOfCheeps { get; set; }
        
        public int TotalPages => GetTotalPages(NumberOfCheeps, PageSize);

        [BindProperty] public required string Message { get; set; } = "";

        public PublicModel(ICheepRepository cheepService, IAuthorRepository authorService, UserManager<Author> userManager)
        {
            _authorService = authorService;
            _cheepService = cheepService;
            _userManager = userManager;
            NumberOfCheeps = Cheeps.Count;
        }

        public async Task<IActionResult> OnGet(int? publicpage = 1)
        {
            CurrentPage = publicpage ?? 1;

            Cheeps = await _cheepService.GetCheeps();
    
            NumberOfCheeps = await _cheepService.GetCheepCount();
            
            CurrentPageCheeps = await _cheepService.GetPaginatedCheeps(CurrentPage - 1, PageSize);
            
            if (User.Identity?.IsAuthenticated == true)
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId != null){
                    ViewData["Following"] = await _authorService.GetFollowedIds(currentUserId);
                }
            }
    
            return Page();
        }
        
        public async Task<IActionResult> OnPostFollow(string authorId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                await _authorService.FollowUser(currentUserId, authorId);
            }
            
            return RedirectToPage(); 
        }
        
        public async Task<IActionResult> OnPostUnfollow(string authorId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                await _authorService.UnfollowUser(currentUserId, authorId);
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

            await _cheepService.PostCheep(Message, user.UserName ?? "unknown", user.Email ??  "unknown");
            return RedirectToPage();
        }
    }
}