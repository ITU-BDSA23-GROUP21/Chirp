using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel {
    private readonly ICheepService _service;

    [FromQuery(Name = "page")]
    public int Pageno { get; set; }

    public IEnumerable<CheepDto> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service) {
        _service = service;
        Cheeps = new List<CheepDto>(0);
    }

    public async Task<ActionResult> OnGet(string author) {
        Cheeps = await _service.GetCheepsFromAuthor(author, Pageno);
        return Page();
    }
    public async Task<IActionResult> OnPostAsync() {
        string? message = Request.Form["NewMessage"];
        if (message != null) {
            FluentValidation.Results.ValidationResult task = await _service.AddCheep(message, User.Identity.Name);
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

