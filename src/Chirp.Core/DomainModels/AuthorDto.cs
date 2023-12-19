namespace Chirp.Core;
/// <summary>
/// Stores and transfers the necessary data of an author from the database to the frontend and vice versa.
/// </summary>
public record AuthorDto(string Name, string Email);