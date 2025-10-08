using System.Collections.Generic;
using Chirp.Razor.DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PaginationModel
{
    private readonly ICheepRepository _service;
    public List<Cheep> Cheeps { get; set; }
    public List<Cheep> CurrentPageCheeps { get; set; }
    public int NumberOfCheeps => Cheeps.Count;
    public int TotalPages => (int)Math.Ceiling((double)NumberOfCheeps / PageSize);
    public UserTimelineModel(ICheepRepository service) 
    {
        _service = service;
    }

    public ActionResult OnGet(string author, string authorID, int ? timelinepage)
    {
        CurrentPage = timelinepage ?? 1;
        CurrentPageCheeps = _service.GetPaginatedCheeps(CurrentPage, PageSize, author);
        Cheeps = _service.GetCheeps(author);
        return Page();
    }
}
