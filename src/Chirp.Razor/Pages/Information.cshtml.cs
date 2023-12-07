using Chirp.Core;

namespace Chirp.Razor.Pages;

public class InformationModel : TimelineModel {
    public InformationModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) {
    }

    protected override Task<List<CheepDto>> GetCheeps() {
        return _cheepService.GetCheepsFromAuthors(new List<string>() {User.Identity.Name}, Pageno, Email);
    }
}