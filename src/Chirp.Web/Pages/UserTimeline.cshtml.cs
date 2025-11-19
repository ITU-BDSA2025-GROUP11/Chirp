using Chirp.Core.DTO;
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

        public List<CheepDTO>? Cheeps { get; set; }
        public List<CheepDTO>? CurrentPageCheeps { get; set; }

        public int NumberOfCheeps => Cheeps?.Count ?? 0;
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
                return RedirectToPage(new { author });
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
    }
}