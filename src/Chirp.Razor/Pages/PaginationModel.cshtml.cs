namespace Chirp.Razor.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class PaginationModel : PageModel
{
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 32;
    //
    //

}