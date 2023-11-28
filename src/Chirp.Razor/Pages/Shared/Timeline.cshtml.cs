using Chirp.Core;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public abstract class TimelineModel : PageModel {

    protected readonly ICheepService _cheepService;
    protected readonly IAuthorService _authorService;

    [FromQuery(Name = "page")]
    public virtual int Pageno { get; set; }

    public IEnumerable<CheepDto> Cheeps { get; set; }
    public IEnumerable<AuthorDto> Followings { get; set; }
    public string? FailedValidationString { get; set; }

    public TimelineModel(ICheepService cheepService, IAuthorService authorService) {
        _cheepService = cheepService;
        _authorService = authorService;
        Cheeps = new List<CheepDto>(0);
        Followings = new List<AuthorDto>(0);
    }

    protected abstract Task<List<CheepDto>> GetCheeps();

    public async Task<ActionResult> OnGet() {
        Cheeps = await GetCheeps();
        var email = User.Claims.Where(c => c.Type == "emails").FirstOrDefault()?.Value;
        Followings = await _authorService.GetFollowings(User?.Identity?.Name, email);
        return Page();
    }

    //fluent validation may not be the right return type since its been casted ealier.
    public async Task<IActionResult> OnPostAsync() {
        string? message = Request.Form["NewMessage"];
        if (message != null && User.Identity?.Name != null) {
            var email = User.Claims.Where(c => c.Type == "emails").Single().Value;
            ValidationResult task = await _cheepService.AddCheep(message, User.Identity.Name, email);
            HandleClientValidation(task);
            Cheeps = await GetCheeps();
        }
        return Page();
    }

    public void HandleClientValidation(ValidationResult results) {
        if (!results.IsValid) {
            FailedValidationString = "\n";
            foreach (var failure in results.Errors) {
                FailedValidationString += "Message not valid. Error was: " + failure.ErrorMessage + "\n";
            }
        }
        else {
            Console.WriteLine("Valid");
            FailedValidationString = null;
        }
    }

    public bool IsFollowing(string following) {
        return Followings.Any(author => author.Name == following);
    }

    public async Task<IActionResult> OnPostUnFollowAsync(string author) {
        var userName = (User.Identity?.Name) ?? throw new Exception("Error attempting to follow when user is not logged in");
        await _authorService.UnFollow(userName, author);
        return RedirectToPage(new { author = userName });
    }

    public async Task<IActionResult> OnPostFollowAsync(string author) {
        var userName = (User.Identity?.Name) ?? throw new Exception("Error attempting to follow when user is not logged in");
        await _authorService.Follow(userName, author);
        return RedirectToPage();
    }
}
