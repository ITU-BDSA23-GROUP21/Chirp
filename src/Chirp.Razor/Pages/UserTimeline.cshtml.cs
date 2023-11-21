using Chirp.Core;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : TimelineModel {
    public UserTimelineModel(ICheepService service) : base(service) { }

    protected override Task<List<CheepDto>> GetCheeps() {
        // We access route data directly here, as saving it as a property in OnGet, did not make it available in OnPost
        // Is this the right way to access route data?
        return _cheepService.GetCheepsFromAuthor(RouteData?.Values?["author"]?.ToString(), Pageno);
    }
}

