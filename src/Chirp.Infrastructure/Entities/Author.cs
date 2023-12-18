namespace Chirp.Infrastructure;
/// <summary>
/// Entity that represents a user in the system.
/// <para> Authors can write, like and dislike cheeps, as well as follow and be followed by other authors.</para>
/// </summary>
public class Author {

    /// <value> Property <c>Id</c> is a global unique identifier, created by the datatype 
    /// <seealso cref="Guid " href=" https://learn.microsoft.com/en-us/dotnet/api/system.guid?view=net-8.0"/>
    /// </value>
    public Guid Id { get; set; }

    /// <value> Property <c>Email</c> represents the email of the author </value>
    public required string Email { get; set; }

    /// <value>Property <c>Name</c> represents the name of the author </value>
    public required string Name { get; set; }

    /// <value> Property <c>Cheeps</c> is a collection of the cheeps created by the author </value>
    public required IEnumerable<Cheep> Cheeps { get; set; }

    /// <value> Property <c>Followers</c> is a collection of authors that follows this author </value>
    public required ICollection<Author> Followers { get; set; }

    /// <value> Property <c>Following</c> is a collection of authors that this author follows </value>
    public required ICollection<Author> Following { get; set; }

    /// <value> Property <c>Likes</c> is a collection of like entities</value>
    public ICollection<Like> Likes {get; set; }
    
    /// <summary>
    /// This constructor initializes the <see cref="Likes"/> collection to a new list.
    /// </summary>
    public Author() {
        Likes = new List<Like>();
    }
}
