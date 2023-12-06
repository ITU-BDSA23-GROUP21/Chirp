namespace Chirp.Infrastructure;

public class Cheep {
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public required Author Author {get; set;}
    public ICollection<Like> Likes { get; set;}
}

