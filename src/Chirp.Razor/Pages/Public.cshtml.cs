using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;

    [FromQuery(Name = "page")]
    public int Pageno { get; set; }

    public IEnumerable<CheepDto> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet()
    {
        Cheeps = _service.GetCheeps(Pageno);
        Console.WriteLine(Pageno);
        return Page();
    }
}
