using System.Security.Claims;
using Chirp.Core.DTO;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Chirp.Core.DomainModel;
using Humanizer;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Web.Pages
{
    public class PublicModel : PaginationModel
    {
        private readonly IAuthorService _authorService;
        private readonly ICheepService _cheepService;
        
        public List<CheepDTO> CurrentPageCheeps { get; set; } = new();
        
        public List<string> Following { get; set; } = new();

        public List<string> Likes { get; set; } = new();

        public List<string> Dislikes { get; set; } = new();
        
        public List<CheepDTO> Cheeps { get; set; } = new();

        public int NumberOfCheeps { get; set; }
        
        public int TotalPages => GetTotalPages(NumberOfCheeps, PageSize);

        [BindProperty] public required string Message { get; set; } = "";

        public PublicModel(ICheepService cheepService, IAuthorService authorService)
        {
            _authorService = authorService;
            _cheepService = cheepService;
            NumberOfCheeps = Cheeps.Count;
            Message = "";
        }

        public async Task<IActionResult> OnGet(int? publicpage = 1)
        {
            CurrentPage = publicpage ?? 1;

            Cheeps = await _cheepService.GetCheeps();
    
            NumberOfCheeps = await _cheepService.GetCheepCount();
            
            CurrentPageCheeps = await _cheepService.GetPaginatedCheeps(CurrentPage, PageSize);
            
            if (User.Identity?.IsAuthenticated == true)
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId != null){
                    ViewData["Following"] = await _authorService.GetFollowedIds(currentUserId);
                    ViewData["LikedCheeps"] = await _authorService.GetLikedCheepIds(currentUserId);
                    ViewData["DislikedCheeps"] = await _authorService.GetDislikedCheepIds(currentUserId);
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
        
        public async Task<IActionResult> OnPostLike(int cheepId)
        {
            Console.WriteLine("I AM LIKING CHEEP: " + cheepId);
            
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
        

        
        public async Task<IActionResult> OnPostUnDislike(int cheepId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                await _cheepService.RemoveDislike(currentUserId, cheepId);
            }
        
            return RedirectToPage();
        }
        
        public int GetTotalPages(int numberOfCheeps, int pageSize)
        {
            return (int)Math.Ceiling((double)numberOfCheeps / pageSize);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Message))
            {
                return RedirectToPage();
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Challenge();
            }

            await _cheepService.PostCheep(Message, currentUserId);
            return RedirectToPage();
        }
    }
}