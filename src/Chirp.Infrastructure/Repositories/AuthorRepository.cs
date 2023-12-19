using Chirp.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository {
    private readonly ChirpContext _dbContext;
    private readonly AuthorValidator _authorValidator = new();
    public AuthorRepository(ChirpContext dbContext) => _dbContext = dbContext;

    public Task<AuthorDto> GetAuthorByName(string name) {
        var author = _dbContext.Authors
            .Where(author => author.Name == name)
            .Select(author => new AuthorDto(author.Name, author.Email))
            .FirstAsync();

        return author;

    }
    public async Task CreateAuthor(AuthorDto author) {
        var authorExists = await _dbContext.Authors.AnyAsync(a => a.Email == author.Email);

        if (!authorExists) {
            var dbAuthor = new Author() {
                Id = Guid.NewGuid(),
                Email = author.Email,
                Name = author.Name,
                Cheeps = new List<Cheep>(),
                Followers = new List<Author>(),
                Following = new List<Author>()
            };

            var results = _authorValidator.Validate(dbAuthor);

            if (results.IsValid) {
                await _dbContext.Authors.AddAsync(dbAuthor);
                _dbContext.SaveChanges();
            } else {
                throw new InvalidOperationException("Invalid name or email for author");
            }
        }
    }

    public async Task<IEnumerable<AuthorDto>> GetFollowings(string name) {
        var author = await _dbContext.Authors
            .Where(author => author.Name == name)
            .Include(author => author.Following)
            .FirstOrDefaultAsync();

        if (author?.Following == null) {
            return Enumerable.Empty<AuthorDto>();
        }

        return author.Following.Select(author => new AuthorDto(author.Name, author.Email));
    }

    public async Task Follow(string followerName, string followingName) {
        var followingAuthor = await _dbContext.Authors
            .Where(author => author.Name == followingName)
            .Include(author => author.Followers)
            .FirstAsync();

        var followerAuthor = await _dbContext.Authors
            .Where(author => author.Name == followerName)
            .Include(author => author.Followers)
            .FirstAsync();

        followingAuthor.Followers ??= new List<Author>();

        followingAuthor.Followers.Add(followerAuthor);
        _dbContext.SaveChanges();
    }

    public async Task UnFollow(string followerName, string followingName) {
        var followingAuthor = await _dbContext.Authors
            .Where(author => author.Name == followingName)
            .Include(author => author.Followers)
            .FirstAsync();

        var followerAuthor = await _dbContext.Authors
            .Where(author => author.Name == followerName)
            .Include(author => author.Followers)
            .FirstAsync();

        followingAuthor.Followers.Remove(followerAuthor);
        _dbContext.SaveChanges();
    }

    /// <summary>
    /// Authors can delete their information from the system
    /// In this implementation users are anonymized by removing any identifiable information about the author
    /// </summary>
    public async Task Anonymize(string name) {
        var author = await _dbContext.Authors
            .Where(author => author.Name == name)
            .FirstAsync();

        Guid guid = author.Id;
        author.Name = guid.ToString("D");
        author.Email = guid.ToString("D");

        _dbContext.SaveChanges();
    }
}

/// <summary>
/// This validator is used when creating an author
/// Used to make sure, no author that would violate the database criteria is added.
/// If the validation fails the error message is also used to tell the user what was wrong about the author
/// </summary>
public class AuthorValidator : AbstractValidator<Author> {
    public AuthorValidator() {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
    }
}
