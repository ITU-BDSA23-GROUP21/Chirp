using Chirp.Core;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : TimelineModel {
    public UserTimelineModel(ICheepService service) : base(service) {}

    protected override Task<List<CheepDto>> GetCheeps() {
        return _service.GetCheepsFromAuthor(RouteData.Values["author"].ToString(), Pageno);
    }
}

