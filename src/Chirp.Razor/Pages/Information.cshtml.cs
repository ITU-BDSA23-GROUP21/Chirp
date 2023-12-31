using Chirp.Core;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Razor.Pages;

/// <summary>
/// Class that gathers information about the authorized user
/// </summary>
public class InformationModel : TimelineModel {
    public InformationModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) {
    }

    protected override Task<List<CheepDto>> GetCheeps() {
        var authorName = User.Identity?.Name;
        if (authorName == null) return Task.FromResult( new List<CheepDto>());
        return _cheepService.GetCheepsFromAuthors(new List<string>() { authorName }, Pageno, Email);
    }

    public async Task<IActionResult> OnPostAnonymizeAsync() {
        if (User.Identity?.Name == null) return RedirectToPage();
        await _authorService.Anonymize(User.Identity.Name);
        string domainName = HttpContext.Request.Host.Value;
        return Redirect("http://" + domainName + "/MicrosoftIdentity/Account/SignOut");
    }
}