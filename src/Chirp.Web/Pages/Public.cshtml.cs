using System.Collections.Generic;
using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Chirp.Razor.DomainModel;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PaginationModel
{
    private readonly ICheepRepository _service;
    public List<Cheep> Cheeps => _service.GetCheeps();
    public List<Cheep> CurrentPageCheeps { get; set; }
    public int NumberOfCheeps => Cheeps.Count;
    public int TotalPages => GetTotalPages(NumberOfCheeps, PageSize);

    public PublicModel(ICheepRepository service)
    {
        _service = service;
    }
    public ActionResult OnGet(int? publicpage = 1)
    {
        CurrentPage = publicpage ?? 1;
        CurrentPageCheeps = _service.GetPaginatedCheeps(CurrentPage, PageSize);
        return Page();
    }

    public int GetTotalPages(int numberOfCheeps, int pageSize)
    {
        return (int)Math.Ceiling((double)numberOfCheeps / pageSize);
    }
}
