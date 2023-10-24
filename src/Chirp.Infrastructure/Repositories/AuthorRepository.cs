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
        await _dbContext.Authors.AddAsync(new Author() {Id = Guid.NewGuid(), Email = author.Email, Name = author.Name, Cheeps = new List<Cheep>()});
        _dbContext.SaveChanges();
    }
}