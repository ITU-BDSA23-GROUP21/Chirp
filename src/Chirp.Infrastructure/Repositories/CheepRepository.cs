using Chirp.Core;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository {
    private readonly ChirpContext _dbContext;
    private NewCheepValidator newCheepValidator = new();

    public CheepRepository(ChirpContext dbContext) =>
        _dbContext = dbContext;

    public Task<List<CheepDto>> GetCheeps(int page, string? author = null, string? userEmail = null) {
        if (page <= 0) page = 1;
        return _dbContext.Cheeps
            .Where(cheep => cheep.Author.Name == author || author == null)
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
        var author = await _dbContext.Authors.Where(author => author.Name == authorName).FirstOrDefaultAsync();

        if (author == null) {
            author = new Author() {
                Id = Guid.NewGuid(),
                Name = authorName,
                Email = email,
                Cheeps = new List<Cheep>(),
                Followers = new List<Author>(),
                Following = new List<Author>()
            };

            await _dbContext.Authors.AddAsync(author);
        }
        var cheep = new Cheep() { Id = Guid.NewGuid(), AuthorId = author.Id, Author = author, Text = message, TimeStamp = DateTime.UtcNow.AddHours(1) }; // Using local datetime will give the wrong time since the servers local time is not equal to ours.
        ValidationResult results = newCheepValidator.Validate(cheep);
        if (results.IsValid) {
            await _dbContext.Cheeps.AddAsync(cheep);
            _dbContext.SaveChanges();
            return results;
        }
        return results;

    }

    public async Task LikeCheep(string userEmail, string cheepId, bool like) {
        throw new NotImplementedException();
    }

    public async Task RemoveLike(string userEmail, string cheepId) {
        throw new NotImplementedException();
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




