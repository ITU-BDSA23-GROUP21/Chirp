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
    /// <returns>A task containing a list of CheepDtos </returns>
    Task<List<CheepDto>> GetCheeps(int page, IEnumerable<string> authors, string? userEmail = null);

    Task<ValidationResult> AddCheep(string message, string authorName);

    /// <summary>
    /// Adds a like entity to the database
    /// </summary>
    /// <param name="userEmail">Email of the current user</param>
    /// <param name="cheepId">Id of the cheep thats being liked or disliked</param>
    /// <param name="value">False if the cheep is disliked, true if the cheep is liked</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task LikeCheep(string userEmail, string cheepId, bool value);

    /// <summary>
    /// Removes a like entity from the database
    /// </summary>
    /// <param name="userEmail">Email of the current user</param>
    /// <param name="cheepId">Id of the cheep that has its like or dislike removed</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task RemoveLike(string userEmail, string cheepId);
}