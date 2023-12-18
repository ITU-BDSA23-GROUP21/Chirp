namespace Chirp.Core;
/// <summary>
/// Values from an author entity needed by the frontend.
/// </summary>
/// <param name="Name">Name of the author</param>
/// <param name="Email">Email of the author</param>
public record AuthorDto(string Name, string Email);