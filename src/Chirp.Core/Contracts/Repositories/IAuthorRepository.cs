namespace Chirp.Core;

public interface IAuthorRepository {
    Task<AuthorDto> GetAuthorByName(string name);
    Task<AuthorDto> GetAuthorByEmail(string email);
    Task CreateAuthor(AuthorDto author);
}