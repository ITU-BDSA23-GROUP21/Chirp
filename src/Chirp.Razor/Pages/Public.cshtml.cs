using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        Console.WriteLine(Pageno);
        return Page();
    }
    //fluent validation may not be the right return type since its been casted ealier.
    public async Task<FluentValidation.Results.ValidationResult> OnPostAsync() {
        string message = Request.Form["Text"];
        if (message != null) {
            return await _service.AddCheep(message, User.Identity.Name);
        }
        return null;
    }
}
