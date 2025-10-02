using Microsoft.AspNetCore.Mvc;
using Models;

namespace Chirp.Razor.Pages;

public class PublicModel : PaginationModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }
    public List<CheepViewModel> CurrentPageCheeps { get; set; }
    
    public int PageCount => Cheeps.Count / PageSize; 
    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(int? id)
    {
        CurrentPage = id ?? 1;
        Cheeps = _service.GetCheeps();
        CurrentPageCheeps = _service.GetPaginatedCheeps(CurrentPage, PageSize);
        return Page();
    }
}
