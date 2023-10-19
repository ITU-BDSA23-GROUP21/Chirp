using Chirp.Core;

namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository {
    
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