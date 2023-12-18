using Chirp.Core;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository {
    private readonly ChirpContext _dbContext;
    private readonly NewCheepValidator newCheepValidator = new();

    public CheepRepository(ChirpContext dbContext) =>
        _dbContext = dbContext;

    public Task<List<CheepDto>> GetCheeps(int page) {
        var authors = Enumerable.Empty<string>();
        return GetCheeps(page, authors);
    }

    public Task<List<CheepDto>> GetCheeps(int page, string? author, string? userEmail = null) {
        List<string> authors = new();
        if (author != null) {
            authors.Add(author);
        }
        return GetCheeps(page, authors, userEmail);
    }

    // Returns the most recent 32 cheeps. Also take a list of authors to filter cheeps by, and an optional userEmail.
    // If userEmail is supplied, it is also included whether that user has like the cheeps
    public Task<List<CheepDto>> GetCheeps(int page, IEnumerable<string> authors, string? userEmail = null) {
        if (page <= 0) page = 1;
        return _dbContext.Cheeps
            .Where(cheep => !authors.Any() || authors.Contains(cheep.Author.Name))
            .OrderByDescending(cheep => cheep.TimeStamp)
            .Skip(32 * (page - 1))
            .Take(32)
            .Include(cheep => cheep.Likes)
            .ThenInclude(like => like.Author)
            .Select(cheep => new CheepDto(
                cheep.Id.ToString(),
                cheep.Author.Name,
                cheep.Text,
                cheep.TimeStamp.ToString("MM/dd/yy H:mm:ss"),
                cheep.Likes.Where(l => l.Liked).Count() - cheep.Likes.Where(l => !l.Liked).Count(),
                userEmail == null || !cheep.Likes.Any(like => like.Author.Email == userEmail)
                    ? null
                    : cheep.Likes.First(like => like.Author.Email == userEmail).Liked
            ))
            .ToListAsync();
    }

    public async Task<ValidationResult> AddCheep(string message, string authorName, string email) {
        var author = await _dbContext.Authors.Where(author => author.Name == authorName).FirstAsync();

        var cheep = new Cheep() { 
            Id = Guid.NewGuid(),
            AuthorId = author.Id,
            Author = author,
            Text = message,
            TimeStamp = DateTime.UtcNow.AddHours(1)
            // Using local datetime will give the wrong time since the servers local time is not equal to ours.
            // This is a temporary workaround, that will not work if the servers or users change timezone
        };
        ValidationResult results = newCheepValidator.Validate(cheep);
        if (results.IsValid) {
            await _dbContext.Cheeps.AddAsync(cheep);
            _dbContext.SaveChanges();
            return results;
        }
        return results;

    }

    // Used for liking or disliking cheeps. The parameter 'Value' determnines whether its a like or dislike
    public async Task LikeCheep(string userEmail, string cheepId, bool value) {
        var author = await _dbContext.Authors.Where(author => author.Email == userEmail).FirstAsync();
        var cheep = await _dbContext.Cheeps.Where(cheep => cheep.Id == Guid.Parse(cheepId)).Include(cheep => cheep.Likes).FirstAsync();
        var like = cheep.Likes.Where(like => like.AuthorId == author.Id).FirstOrDefault();

        cheep.Likes ??= new List<Like>();
        if (like == null) {
            cheep.Likes.Add(new Like() { Liked = value, Author = author, Cheep = cheep, AuthorId = author.Id, CheepId = cheep.Id });
        }
        else {
            like.Liked = value;
        }

        _dbContext.SaveChanges();
    }

    // Used for removing a like or a dislike for a cheep
    public async Task RemoveLike(string userEmail, string cheepId) {
        var cheep = await _dbContext.Cheeps.Where(cheep => cheep.Id == Guid.Parse(cheepId)).Include(cheep => cheep.Likes).FirstAsync();
        var author = await _dbContext.Authors.Where(author => author.Email == userEmail).FirstAsync();
        var like = cheep.Likes.First(l => l.AuthorId == author.Id);

        cheep.Likes.Remove(like);

        _dbContext.SaveChanges();
    }
}


public class NewCheepValidator : AbstractValidator<Cheep> {
    public NewCheepValidator() {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.AuthorId).NotEmpty();
        RuleFor(x => x.Author).NotEmpty();
        RuleFor(x => x.Author.Name).NotEmpty();
        RuleFor(x => x.Author.Email).EmailAddress();
        RuleFor(x => x.Text).Length(1, 160).NotEmpty();
    }
}




