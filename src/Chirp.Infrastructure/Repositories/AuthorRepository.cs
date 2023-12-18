using Chirp.Core;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository {

    private readonly ChirpContext _dbContext;
    public AuthorRepository(ChirpContext dbContext) => _dbContext = dbContext;

    public Task<AuthorDto> GetAuthorByName(string name) {
        var author = _dbContext.Authors
            .Where(author => author.Name == name)
            .Select(author => new AuthorDto(author.Name, author.Email))
            .FirstAsync();

        return author;

    }
    public Task<AuthorDto> GetAuthorByEmail(string email) {
        return _dbContext.Authors
            .Where(author => author.Email == email)
            .Select(author => new AuthorDto(author.Name, author.Email))
            .FirstAsync();
    }
    public async Task CreateAuthor(AuthorDto author) {
        var authorExists = await _dbContext.Authors.AnyAsync(a => a.Email == author.Email);

        if (!authorExists) {
            await _dbContext.Authors.AddAsync(new Author() {
                Id = Guid.NewGuid(),
                Email = author.Email,
                Name = author.Name,
                Cheeps = new List<Cheep>(),
                Followers = new List<Author>(),
                Following = new List<Author>()
            }
        );
        _dbContext.SaveChanges();
        }
    }

    public async Task<IEnumerable<AuthorDto>> GetFollowings(string name, string email) {
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
