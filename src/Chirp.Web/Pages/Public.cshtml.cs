using System.Collections.Generic;
using Chirp.Core.DTOs;
using Chirp.Infrastructure;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Razor.Pages
{
    public class PublicModel : PaginationModel
    {
        private readonly ICheepRepository _service;

        public List<CheepDTO> Cheeps => _service.GetCheeps();
        public List<CheepDTO> CurrentPageCheeps { get; set; } = new();
        public int NumberOfCheeps => Cheeps.Count;
        public int TotalPages => GetTotalPages(NumberOfCheeps, PageSize);

        public PublicModel(ICheepRepository service)
        {
            _service = service;
        }

        public ActionResult OnGet(int? publicpage = 1)
        {
            CurrentPage = publicpage ?? 1;
            CurrentPageCheeps = _service.GetPaginatedCheeps(CurrentPage - 1, PageSize);
            return Page();
        }

        private int GetTotalPages(int numberOfCheeps, int pageSize)
        {
            return (int)Math.Ceiling((double)numberOfCheeps / pageSize);
        }
    }
}