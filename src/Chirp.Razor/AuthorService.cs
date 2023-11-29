using Chirp.Core;

namespace Chirp.Razor;

public interface IAuthorService {
    // Validation?
    public Task Follow(string followerName, string followingName);

    public Task UnFollow(string followerName, string followingName);

    public Task<IEnumerable<AuthorDto>> GetFollowings(string? name, string? email);

    public Task Anonymize(string name);

    public Task<IEnumerable<string>> GetFollowingsLinks(string name, string email);

    public Task<AuthorDto> GetAuthorByName(string name);
}

public class AuthorService : IAuthorService {

    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository) => _authorRepository = authorRepository;
    public async Task Follow(string followerName, string followingName) {
        await _authorRepository.Follow(followerName, followingName);
    }

    public async Task UnFollow(string followerName, string followingName) {
        await _authorRepository.UnFollow(followerName, followingName);
    }

    public async Task<IEnumerable<AuthorDto>> GetFollowings(string? name, string? email) {
        if (name == null || email == null) {
            return Enumerable.Empty<AuthorDto>();
        }
        return await _authorRepository.GetFollowings(name, email);
    }

    public async Task Anonymize(string name) => await _authorRepository.Anonymize(name);

    public async Task<IEnumerable<string>> GetFollowingsLinks(string name, string email) => await _authorRepository.GetFollowingsLinks(name, email);

    public Task<AuthorDto> GetAuthorByName(string name) => _authorRepository.GetAuthorByName(name);
}