namespace Chirp.Core;
/// <summary>
/// Stores and transfers the necessary data of an author from the database to the frontend and vice versa.
/// </summary>
/// <param name="Name">Name of the author</param>
/// <param name="Email">Email of the author</param>
public record AuthorDto(string Name, string Email);