using Chirp.Core;

namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository {

    private readonly ChirpContext _dbContext;

    public AuthorRepository(ChirpContext dbContext) => _dbContext = dbContext;
    
    public Task<AuthorDto> GetAuthorByName(string name) {
        throw new NotImplementedException();
    }
    public Task<AuthorDto> GetAuthorByEmail(string email) {
        throw new NotImplementedException();
    }
    public Task CreateAuthor(AuthorDto author) {
        throw new NotImplementedException();
    }
}