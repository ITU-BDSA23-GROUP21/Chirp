namespace Chirp.Infrastructure;
/// <summary>
/// Entity that represent a like or dislike given to a cheep by an author.
/// </summary>
public class Like {
    public Guid CheepId {get; set; }
    public Guid AuthorId {get; set; }
    public required Cheep Cheep {get; set;}
    public required Author Author {get; set;}
    // Is true if a cheep is liked, false if it is disliked, and null if neither
    public required bool Liked {get; set; }
}
