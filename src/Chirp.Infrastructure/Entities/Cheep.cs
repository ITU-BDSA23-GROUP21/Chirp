namespace Chirp.Infrastructure;
/// <summary>
/// Entity that represents a cheep in the Chirp application
/// <para> Cheeps are a message to the system created by an author</para>
/// </summary>
public class Cheep {

    // Guid is used to insure a unique identifier for all cheeps
    // see "https://learn.microsoft.com/en-us/dotnet/api/system.guid?view=net-8.0"
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public required Author Author { get; set; }
    public ICollection<Like> Likes { get; set; }
    public Cheep() {
        Likes = new List<Like>();
    }
}

