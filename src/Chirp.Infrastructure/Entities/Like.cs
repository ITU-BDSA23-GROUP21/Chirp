namespace Chirp.Infrastructure;
/// <summary>
/// Entity that represent a like or dislike given to a cheep by an author.
/// </summary>
public class Like {

    /// <value> Property <c>CheepId</c> represents the id of the cheep that has been liked or disliked</value>
    public Guid CheepId {get; set; }

    /// <value> Property <c>AuthorId</c> represents the id of the author who liked or disliked</value>
    public Guid AuthorId {get; set; }

    /// <value> Property <c>Cheep</c> is a reference to the cheep entity that has been liked or disliked</value>
    public required Cheep Cheep {get; set;}

    /// <value> Property <c>Author</c> is a reference to the author entity who liked or disliked</value>
    public required Author Author {get; set;}

    /// <value> Property <c>Liked</c> represent if the a cheep is liked or disliked 
    /// <para> Is true if liked, false if disliked, and null if neither.</para>
    /// </value>
    public required bool Liked {get; set; }
}
