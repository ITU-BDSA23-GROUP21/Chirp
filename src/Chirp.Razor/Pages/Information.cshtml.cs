using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Chirp.Razor.Pages;

public class InformationModel : TimelineModel {
    public InformationModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) {
    }

    public async Task<IActionResult> OnPostAnonymizeAsync(string name) {
        await _authorService.Anonymize(name);
        return RedirectToPage();

    }

    protected override Task<List<CheepDto>> GetCheeps() {
        return _cheepService.GetCheeps(Pageno);
    }

}