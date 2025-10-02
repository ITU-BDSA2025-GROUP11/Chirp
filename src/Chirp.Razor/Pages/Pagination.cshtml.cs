using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PaginationModel : PageModel
{
    public int CurrentPage {get; set;}
    public int PageSize { get; set; } = 32; // cheeps per page
    
    public int NumberOfCheeps {get; set;}
    
    // how many pages depends on how many cheeps in total + cheeps per page
    public int PageCount =>  NumberOfCheeps / PageSize;  
    
    
}