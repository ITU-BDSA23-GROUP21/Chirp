namespace Chirp.Infrastructure;

public class Like {
    public Guid CheepId {get; set; }
    public Guid AuthorId {get; set; }

    public required Cheep Cheep {get; set;}
    public required Author Author {get; set;}
    public required bool Liked {get; set; }
}
