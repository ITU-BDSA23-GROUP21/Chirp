﻿using Chirp.Core;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : TimelineModel {
    public UserTimelineModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) { }

    protected override async Task<List<CheepDto>> GetCheeps() {
        // We access route data directly here, as saving it as a property in OnGet, did not make it available in OnPost
        // Is this the right way to access route data?
        var author = RouteData?.Values?["author"]?.ToString();

        if (User.Identity != null && User.Identity.Name == author) {
            IEnumerable<AuthorDto> followings = await _authorService.GetFollowings(User.Identity.Name, Email);
            var authorNames = followings.Select(f => f.Name).Append(author);
            return await _cheepService.GetCheepsFromAuthors(authorNames, Pageno, Email);
        }
        else {
            return await _cheepService.GetCheepsFromAuthor(author, Pageno, Email);
        }

    }
}

