using Chirp.Core;

namespace Chirp.Razor.Pages;

public class PublicModel : TimelineModel {
    public PublicModel(ICheepService service): base(service) {}
    
    protected override Task<List<CheepDto>> GetCheeps() {
        if(Pageno <= 0) Pageno = 1;
        return _service.GetCheeps(Pageno);
    }
}

