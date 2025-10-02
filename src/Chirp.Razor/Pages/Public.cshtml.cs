using Microsoft.AspNetCore.Mvc;
using Models;

namespace Chirp.Razor.Pages;

public class PublicModel : PaginationModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }
    
    public int PageCount => Cheeps.Count / PageSize; 
    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet()
    {
        Cheeps = _service.GetPaginatedCheeps(CurrentPage, PageSize);
        return Page();
    }
}
