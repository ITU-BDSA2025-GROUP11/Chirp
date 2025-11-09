using System.Collections.Generic;
using Chirp.Core.DTOs;
using Chirp.Infrastructure;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Chirp.Core.DomainModel;
using System.Threading.Tasks;

namespace Chirp.Razor.Pages
{
    public class PublicModel : PaginationModel
    {
        private readonly ICheepRepository _service;
        private readonly UserManager<Author> _userManager;

        public List<CheepDTO> Cheeps => _service.GetCheeps();
        public List<CheepDTO> CurrentPageCheeps { get; set; } = new();
        public int NumberOfCheeps => Cheeps.Count;
        public int TotalPages => GetTotalPages(NumberOfCheeps, PageSize);

        [BindProperty] public string Message { get; set; }

        public PublicModel(ICheepRepository service, UserManager<Author> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        public ActionResult OnGet(int? publicpage = 1)
        {
            CurrentPage = publicpage ?? 1;
            CurrentPageCheeps = _service.GetPaginatedCheeps(CurrentPage - 1, PageSize);
            return Page();
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

            _service.PostCheep(Message, user.UserName, user.Email);

            // Redirect to the same page to show the new cheep
            return RedirectToPage();
        }
    }
}