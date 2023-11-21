using Chirp.Core;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository {

    private readonly ChirpContext _dbContext;

    public AuthorRepository(ChirpContext dbContext) => _dbContext = dbContext;

    public Task<AuthorDto> GetAuthorByName(string name) {
        return _dbContext.Authors
            .Where(author => author.Name == name)
            .Select(author => new AuthorDto(author.Name, author.Email))
            .FirstAsync();
    }
    public Task<AuthorDto> GetAuthorByEmail(string email) {
        return _dbContext.Authors
            .Where(author => author.Email == email)
            .Select(author => new AuthorDto(author.Name, author.Email))
            .FirstAsync();
    }
    public async Task CreateAuthor(AuthorDto author) {
        await _dbContext.Authors.AddAsync(new Author() { Id = Guid.NewGuid(), Email = author.Email, Name = author.Name, Cheeps = new List<Cheep>() });
        _dbContext.SaveChanges();
    }

    public async Task<IEnumerable<AuthorDto>> GetFollowings(string name) {
        var followings = await _dbContext.Authors
            .Where(author => author.Name == name)
            .Select(author => author.Following)
            .FirstAsync();

        return followings.Select(author => new AuthorDto(author.Name, author.Email));
    }

    public async Task Follow(string followerName, string followingName) {
        var author = await _dbContext.Authors
            .Where(author => author.Name == followingName)
            .FirstAsync();

        // author.Followers.Append()
        throw new NotImplementedException();
        // List<CheepDto> cheep = _dbContext.Where(cheep => cheep.Author.Name =)
    }

    public Task UnFollow(string followerName, string followingName) {
        throw new NotImplementedException();
    }
}