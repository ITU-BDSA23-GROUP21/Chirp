using FluentValidation.Results;

namespace Chirp.Core;

/// <summary>
/// Defines methods used to operate on the cheep entity
/// </summary>
public interface ICheepRepository {

    /// <param name="page">Page number</param>
    /// <param name="authors">A list of authors, used to only get cheeps made by the user and the authors they follow</param>
    /// <param name="userEmail">"Optional; if provided, information about whether the user has liked or disliked the cheep is included."</param>
    /// <returns>The cheeps that belong on the given page number</returns>
    Task<List<CheepDto>> GetCheeps(int page, IEnumerable<string> authors, string? userEmail = null);

    Task<ValidationResult> AddCheep(string message, string authorName);

    /// <summary>
    /// This method is used both to like and dislike cheeps
    /// </summary>
    /// <param name="value">The cheep is liked if this is true, and disliked if it is false</param>
    Task LikeCheep(string userEmail, string cheepId, bool value);
    Task RemoveLike(string userEmail, string cheepId);
}