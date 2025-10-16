using System.Collections.Generic;
using Chirp.Core.DTOs;
using Chirp.Infrastructure;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Razor.Pages
{
    public class UserTimelineModel : PaginationModel
    {
        private readonly ICheepRepository _service;

        public List<CheepDTO> Cheeps { get; set; } = new();
        public List<CheepDTO> CurrentPageCheeps { get; set; } = new();

        public int NumberOfCheeps => Cheeps.Count;
        public int TotalPages => (int)Math.Ceiling((double)NumberOfCheeps / PageSize);

        public UserTimelineModel(ICheepRepository service)
        {
            _service = service;
        }

        public ActionResult OnGet(string author, int? timelinepage)
        {
            CurrentPage = timelinepage ?? 1;

            Cheeps = _service.GetCheeps(author);
            CurrentPageCheeps = _service.GetPaginatedCheeps(CurrentPage - 1, PageSize, author);

            return Page();
        }
    }
}