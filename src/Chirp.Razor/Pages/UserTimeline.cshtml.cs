using Chirp.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : TimelineModel {
    public UserTimelineModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) { }

    protected override Task<List<CheepDto>> GetCheeps() {
        // We access route data directly here, as saving it as a property in OnGet, did not make it available in OnPost
        // Is this the right way to access route data?

        if (User.Identity.Name == RouteData?.Values?["author"]?.ToString()) 
        {
            List<AuthorDto> followings = this.Followings.ToList();
            return _cheepService.GetCheepsFromAuthors(followings,RouteData?.Values?["author"]?.ToString() , Pageno);
        }
        else
        {
            return _cheepService.GetCheepsFromAuthor(RouteData?.Values?["author"]?.ToString(), Pageno);
        }
        
    }
}

