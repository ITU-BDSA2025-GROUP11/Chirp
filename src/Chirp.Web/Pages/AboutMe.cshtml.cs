using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure; 
using Microsoft.AspNetCore.Identity;
using Chirp.Core.DomainModel;
using Chirp.Core.DTO;


namespace Chirp.Web.Pages
{
    public class AboutMeModel : PageModel
    {
        private readonly ICheepService _cheepService;
        private readonly IAuthorService  _authorService;
        private readonly SignInManager<Author> _signInManager;

        public UserInfoDTO? UserInfo { get; private set; }

        public AboutMeModel(ICheepService cheepService, IAuthorService authorService, SignInManager<Author> signInManager)
        {
            _cheepService = cheepService;
            _authorService = authorService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var username = User.Identity?.Name;

            if (username != null)
            {
                UserInfo = await _authorService.GetUserInfo(username);
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostForgetMe()
        {
            var username = User.Identity?.Name;

            if (username != null)
            {
                await _authorService.DeleteUser(username);
                await _signInManager.SignOutAsync();
            }

            return RedirectToPage("/Public"); 
        }
    }
}