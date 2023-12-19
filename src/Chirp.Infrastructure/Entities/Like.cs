namespace Chirp.Infrastructure;
/// <summary>
/// Entity that represent a like or dislike given to a cheep by an author.
/// </summary>
public class Like {
    public Guid CheepId {get; set; }
    public Guid AuthorId {get; set; }
    /// <summary> Needed for Ef core to know <c>CheepId</c> references a cheep in the database</summary>
    public required Cheep Cheep {get; set;}
    /// <summary> Needed for Ef core to know <c>AuthorId</c> references an author in the database </summary>
    public required Author Author {get; set;}
    /// <summary> Is true if a cheep is liked, false if it is disliked, and null if neither </summary>
    public required bool Liked {get; set; }
}
