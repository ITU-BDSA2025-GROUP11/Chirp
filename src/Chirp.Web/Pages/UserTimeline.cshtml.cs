using System.Collections.Generic;
using Chirp.Core.DTOs;
using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace Chirp.Web.Pages
{
    public class UserTimelineModel : PaginationModel
    {
        private readonly ICheepRepository _service;
        private readonly UserManager<Author> _userManager;

        public List<CheepDTO> Cheeps { get; set; } = new();
        public List<CheepDTO> CurrentPageCheeps { get; set; } = new();

        public int NumberOfCheeps => Cheeps.Count;
        public int TotalPages => (int)Math.Ceiling((double)NumberOfCheeps / PageSize);
        
        [BindProperty]
        public string Message { get; set; }

        public UserTimelineModel(ICheepRepository service, UserManager<Author> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        public ActionResult OnGet(string author, int? timelinepage)
        {
            CurrentPage = timelinepage ?? 1;

            Cheeps = _service.GetCheeps(author);
            CurrentPageCheeps = _service.GetPaginatedCheeps(CurrentPage - 1, PageSize, author);

            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync(string author)
        {
            if (string.IsNullOrEmpty(Message))
            {
                return RedirectToPage(new { author = author });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge(); // Not logged in
            }

            _service.PostCheep(Message, user.UserName, user.Email);

            // Redirect to the same author's timeline
            return RedirectToPage(new { author = author });
        }
        public int GetTotalPages(int numberOfCheeps, int pageSize)
        {
            return (int)Math.Ceiling((double)numberOfCheeps / pageSize);
        }
    }
}