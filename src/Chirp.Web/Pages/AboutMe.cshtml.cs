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
        private readonly ICheepRepository _repository;
        private readonly SignInManager<Author> _signInManager;

        public UserInfoDTO? UserInfo { get; private set; }

        public AboutMeModel(ICheepRepository repository,
            SignInManager<Author> signInManager)
        {
            _repository = repository;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var username = User.Identity?.Name;

            if (username != null)
            {
                UserInfo = await _repository.GetUserInfo(username);
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostForgetMe()
        {
            var username = User.Identity?.Name;

            if (username != null)
            {
                await _repository.DeleteUser(username);
                await _signInManager.SignOutAsync();
            }

            return RedirectToPage("/Public"); 
        }
    }
}