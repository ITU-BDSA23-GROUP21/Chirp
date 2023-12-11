using Chirp.Core;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Razor.Pages;

public class InformationModel : TimelineModel {
    public InformationModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) {
    }

    protected override Task<List<CheepDto>> GetCheeps() {
        return _cheepService.GetCheepsFromAuthors(new List<string>() {User.Identity.Name}, Pageno, Email);
    }
    
    //     public async Task<IActionResult> OnPostAnonymizeAsync() {
    //     await _authorService.Anonymize(User.Identity.Name);
    //     string domainName = HttpContext.Request.Host.Value;
    //     return Redirect(domainName + "/MicrosoftIdentity/Account/SignOut");
    // }
}