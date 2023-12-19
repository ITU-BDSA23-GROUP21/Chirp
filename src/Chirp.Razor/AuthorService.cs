using Chirp.Core;

namespace Chirp.Razor;
/// <summary>
/// Defines method used when operation on cheeps in the Chirp application
/// </summary>
public interface IAuthorService {
    // Validation?
    public Task Follow(string userName, string followingName);

    public Task UnFollow(string userName, string followingName);

    public Task<IEnumerable<AuthorDto>> GetFollowings(string? name);

    public Task Anonymize(string name);

    public Task<AuthorDto> GetAuthorByName(string name);
}

public class AuthorService : IAuthorService {

    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository) => _authorRepository = authorRepository;
    public async Task Follow(string userName, string followingName) {
        await _authorRepository.Follow(userName, followingName);
    }

    public async Task UnFollow(string userName, string followingName) {
        await _authorRepository.UnFollow(userName, followingName);
    }

    public async Task<IEnumerable<AuthorDto>> GetFollowings(string? name) {
        if (name == null) {
            return Enumerable.Empty<AuthorDto>();
        }
        return await _authorRepository.GetFollowings(name);
    }

    public async Task Anonymize(string name) => await _authorRepository.Anonymize(name);

    public Task<AuthorDto> GetAuthorByName(string name) => _authorRepository.GetAuthorByName(name);
}