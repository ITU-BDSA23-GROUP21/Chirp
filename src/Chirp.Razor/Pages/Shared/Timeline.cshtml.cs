using Chirp.Core;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public abstract class TimelineModel : PageModel {

    protected readonly ICheepService _service;

    [FromQuery(Name = "page")]
    public virtual int Pageno { get; set; }

    public IEnumerable<CheepDto> Cheeps { get; set; }

    public TimelineModel(ICheepService service) {
        _service = service;
        Cheeps = new List<CheepDto>(0);
    }

    protected abstract Task<List<CheepDto>> GetCheeps();

    public async Task<ActionResult> OnGet() {
        if (Pageno <= 0) Pageno = 1;
        Cheeps = await GetCheeps();
        return Page();
    }

    //fluent validation may not be the right return type since its been casted ealier.
    public async Task<IActionResult> OnPostAsync() {
        string? message = Request.Form["NewMessage"];
        if (message != null && User.Identity?.Name != null) {
            var email = User.Claims.Where(c => c.Type == "emails").Single().Value;
            ValidationResult task = await _service.AddCheep(message, User.Identity.Name, email);
            HandleClientValidation(task);
            Cheeps = await GetCheeps();
        }
        return Page();
    }

    public void HandleClientValidation(ValidationResult task) {
        if (!task.IsValid) {
            Console.WriteLine("Not a valid message");
        }
        else {
            Console.WriteLine("Valid");
        }
    }
}
