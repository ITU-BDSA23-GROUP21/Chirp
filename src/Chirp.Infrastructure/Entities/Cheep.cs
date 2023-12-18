namespace Chirp.Infrastructure;
/// <summary>
/// Entity that reprecents a cheep.
/// <para> Cheeps are a message to the system created by a user</para>
/// </summary>
public class Cheep {

    /// <value> Property <c>Id</c> is a global unique identifier, created by the datatype 
    /// <seealso cref="Guid " href=" https://learn.microsoft.com/en-us/dotnet/api/system.guid?view=net-8.0"/>
    /// </value>
    public Guid Id { get; set; }

    /// <value> Property <c>AuthorId</c> reprecents the id of the author who created this cheep</value>
    public Guid AuthorId { get; set; }

    /// <value> Property <c>Text</c> is the context of the cheep.</value>
    public required string Text { get; set; }

    /// <value> Property <c>TimeStamp</c> describes the time at which the cheep was created using the datetype 
    /// <seealso cref="DateTime" href=" https://learn.microsoft.com/en-us/dotnet/api/system.datetime?view=net-8.0"/>></value>
    public DateTime TimeStamp { get; set; }

    /// <value> Property <c>Author</c> is a reference to the author entity that created this cheep</value>
    public required Author Author { get; set; }

    /// <value> Property <c>Likes</c> is a collection of like entities</value>
    public ICollection<Like> Likes { get; set; }
    
    /// <summary>
    /// This constructor initalizes the <see cref="Likes"/> collection to a new list.
    /// </summary>
    public Cheep() {
        Likes = new List<Like>();
    }
}

