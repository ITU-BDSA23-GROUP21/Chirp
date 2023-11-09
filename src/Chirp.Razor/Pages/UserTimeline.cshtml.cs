using Chirp.Core;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : TimelineModel {
    public UserTimelineModel(ICheepService service) : base(service) {}

    protected override Task<List<CheepDto>> GetCheeps() {
        if(Pageno <= 0) Pageno = 1;
        // We access route data directly here, as saving it as a property in OnGet, did not make it available in OnPost
        return _service.GetCheepsFromAuthor(RouteData?.Values?["author"]?.ToString(), Pageno);
    }
}

