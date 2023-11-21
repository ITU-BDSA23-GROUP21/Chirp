using Chirp.Core;
using Chirp.Infrastructure;

namespace Chirp.Razor;

public interface IAuthorService {
    // Validation?
    public Task Follow(string followerName, string followingName);

    public Task UnFollow(string followerName, string followingName);

    public Task<IEnumerable<AuthorDto>> GetFollowings(string? name, string? email);
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
}