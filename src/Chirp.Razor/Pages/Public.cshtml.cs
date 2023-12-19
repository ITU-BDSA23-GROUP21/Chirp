using Chirp.Core;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Razor.Pages;
/// <summary>
/// Class that receives cheeps by any author to be shown on the public timeline.
/// </summary>
public class PublicModel : TimelineModel {
    public PublicModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) {

    }
    protected override Task<List<CheepDto>> GetCheeps() {
        return _cheepService.GetCheeps(Pageno, Email);
    }

}

