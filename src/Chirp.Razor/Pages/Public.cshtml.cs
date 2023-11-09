using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using FluentValidation;
using System.Security.Claims;

namespace Chirp.Razor.Pages;

public class PublicModel : TimelineModel {
    public PublicModel(ICheepService service): base(service) {

    [FromQuery(Name = "page")]
    public int Pageno { get; set; }

    public IEnumerable<CheepDto> Cheeps { get; set; }

    public PublicModel(ICheepService service) {
        _service = service;
        Cheeps = new List<CheepDto>(0);
    }

    public async Task<ActionResult> OnGet() {
        if(Pageno <= 0) Pageno = 1;
        Cheeps = await _service.GetCheeps(Pageno);
        return Page();
    }
    //fluent validation may not be the right return type since its been casted ealier.
    public async Task<IActionResult> OnPostAsync() {
        string? message = Request.Form["NewMessage"];
        if (message != null) {
            var email = User.Claims.Where(c => c.Type == "emails").Single().Value;
            FluentValidation.Results.ValidationResult task = await _service.AddCheep(message, User.Identity.Name, email);
            HandleClientValidation(task);
            Cheeps = await _service.GetCheeps(Pageno);
        }
        return Page();
    }
    
    protected override Task<List<CheepDto>> GetCheeps() {
        return _service.GetCheeps(Pageno);
    }
}

