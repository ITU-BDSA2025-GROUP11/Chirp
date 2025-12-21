using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages.Errors;

public class Error : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }
}