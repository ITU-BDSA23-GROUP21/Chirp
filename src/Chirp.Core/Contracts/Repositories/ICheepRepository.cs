using FluentValidation.Results;

namespace Chirp.Core;

/// <summary>
/// Defines methods used to operate on the cheep entity
/// </summary>
public interface ICheepRepository {

    /// <summary>
    /// Gets all the cheeps that belongs on a givin page.
    /// </summary>
    /// <param name="page">The page number </param>
    /// <param name="authors">A list of author names used to find cheeps made by specific authors</param>
    /// <param name="userEmail">Email of the current user</param>
    /// <returns></returns>
    Task<List<CheepDto>> GetCheeps(int page, IEnumerable<string> authors, string? userEmail = null);

    Task<ValidationResult> AddCheep(string message, string authorName);

    Task LikeCheep(string userEmail, string cheepId, bool value);

    Task RemoveLike(string userEmail, string cheepId);
}