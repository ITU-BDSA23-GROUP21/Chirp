using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

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
        Cheeps = new List<CheepDto>(0);
    }

    public async Task<ActionResult> OnGet()
    {
        if(Pageno <= 0) Pageno = 1;
        Cheeps = await _service.GetCheeps(Pageno);
        Console.WriteLine(Pageno);
        return Page();
    }
}
