using Chirp.Core;

namespace Chirp.Razor.Pages;

public class PublicModel : TimelineModel {
    public PublicModel(ICheepService service): base(service) {

    }
    protected override Task<List<CheepDto>> GetCheeps() {
        return _cheepService.GetCheeps(Pageno);
    }
}

