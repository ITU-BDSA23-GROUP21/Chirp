using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FluentValidation;
using System.Security.Claims;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel {
    private readonly ICheepService _service;

    [FromQuery(Name = "page")]
    public int Pageno { get; set; }

    public IEnumerable<CheepDto> Cheeps { get; set; }

    public PublicModel(ICheepService service) {
        _service = service;
        Cheeps = new List<CheepDto>(0);
    }

    public async Task<ActionResult> OnGet() {
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

    public void HandleClientValidation(FluentValidation.Results.ValidationResult task) {
        if (!task.IsValid) {
            Console.WriteLine("Not a valid message");
        }
        else {
            Console.WriteLine("Valid");
        }
    }
}

