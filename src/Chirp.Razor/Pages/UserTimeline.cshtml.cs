using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PaginationModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }
    public List<CheepViewModel> CurrentPageCheeps { get; set; }
    public int NumberOfCheeps => Cheeps.Count;
    public int TotalPages => (int)Math.Ceiling((double)NumberOfCheeps / PageSize);
    public UserTimelineModel(ICheepService service)
    {
        _service = service;
        
    }

    public ActionResult OnGet(string author, int ? timelinepage)
    {
        CurrentPage = timelinepage ?? 1;
        CurrentPageCheeps = _service.GetPaginatedCheepsFromAuthor(author, CurrentPage);
        Cheeps = _service.GetCheepsFromAuthor(author);
        return Page();
    }
}
