namespace Chirp.Core;
/// <summary>
/// Defines methods used on and by an author
/// </summary>
public interface IAuthorRepository {

    /// <summary>
    /// Finds an author in the database with the given name
    /// </summary>
    /// <param name="name">Name of the author</param>
    /// <returns>A task containing the AuthorDTO found using the given name</returns>
    Task<AuthorDto> GetAuthorByName(string name);
    Task CreateAuthor(AuthorDto author);

    /// <summary>
    /// This method is used to make an author follow another author
    /// </summary>
    /// <param name="followerName">Name of the author who follows the other author</param>
    /// <param name="followingName">Name of the author who is followed</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task Follow(string followerName, string followingName);

    /// <summary>
    /// This method is used to make an author unfollow another author
    /// </summary>
    /// <param name="followerName">Name of the author who unfollows the other author</param>
    /// <param name="followingName">Name of the author who is unfollowed</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UnFollow(string followerName, string followingName);

    Task<IEnumerable<AuthorDto>> GetFollowings(string name);

    Task Anonymize(string name);
}