using Chirp.Core;

namespace Chirp.Razor.Pages;

public class PublicModel : TimelineModel {
    public PublicModel(ICheepService cheepService, IAuthorService authorService): base(cheepService, authorService) {

    }
    protected override Task<List<CheepDto>> GetCheeps() {
        return _cheepService.GetCheeps(Pageno);
    }
}

