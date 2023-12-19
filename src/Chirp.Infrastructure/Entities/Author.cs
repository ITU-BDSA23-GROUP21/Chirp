namespace Chirp.Infrastructure;
/// <summary>
/// Entity that represents a user of the Chirp application
/// <para> Authors can write, like and dislike cheeps, as well as follow and be followed by other authors </para>
/// <para> All users of the program are made as an author entity when they are authorized for the first time</para>
/// </summary>
public class Author {

    // Guid is used to insure a unique identifier for all authors
    // see "https://learn.microsoft.com/en-us/dotnet/api/system.guid?view=net-8.0"
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required IEnumerable<Cheep> Cheeps { get; set; }
    // Collection of authors that follow this author
    public required ICollection<Author> Followers { get; set; }
    // Collection of authors that this author follows
    public required ICollection<Author> Following { get; set; }
    public ICollection<Like> Likes {get; set; }
    public Author() {
        Likes = new List<Like>();
    }
}
