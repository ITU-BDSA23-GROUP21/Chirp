using Chirp.Core;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Razor.Pages;

public class PublicModel : TimelineModel {
    public PublicModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) {

    }
    protected override Task<List<CheepDto>> GetCheeps() {
        return _cheepService.GetCheeps(Pageno);
    }

    public async Task<IActionResult> OnPostAnonymizeAsync() {
        Console.WriteLine("hey");
        await _authorService.Anonymize("Jacqualine Gilcoine");
        return RedirectToPage();

    }
}

