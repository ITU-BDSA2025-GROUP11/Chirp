using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PaginationModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps => _service.GetCheeps();
    public List<CheepViewModel> CurrentPageCheeps { get; set; }
    public int NumberOfCheeps => Cheeps.Count;
    public int TotalPages => (int)Math.Ceiling((double)NumberOfCheeps / PageSize);

    public PublicModel(ICheepService service)
    {
        _service = service;
    }
    public ActionResult OnGet(int? id = 1)
    {
        //Cheeps = _service.GetCheeps();
        CurrentPage = id ?? 1;
        CurrentPageCheeps = _service.GetPaginatedCheeps(CurrentPage, PageSize);
        return Page();
    }
}
