using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace Chirp.Razor.Pages;

public class PaginationModel : PageModel
{
   [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 32; // cheeps per page
    
    public bool ShowPrevious =>  CurrentPage > 1 ;
    public bool ShowNext =>  CurrentPage < PageSize;
    
    //public int NumberOfCheeps {get; set;}
    
    // how many pages depends on how many cheeps in total + cheeps per page
    //public int PageCount =>  NumberOfCheeps / PageSize;
    
    
}