namespace Chirp.Core;
/// <summary>
/// 
/// </summary>
public interface IAuthorRepository {
    Task<AuthorDto> GetAuthorByName(string name);
    Task CreateAuthor(AuthorDto author);
    Task Follow(string followerName, string followingName);

    Task UnFollow(string followerName, string followingName);

    Task<IEnumerable<AuthorDto>> GetFollowings(string name);

    Task Anonymize(string name);
}