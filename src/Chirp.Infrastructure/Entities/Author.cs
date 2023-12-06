namespace Chirp.Infrastructure;
public class Author {
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required IEnumerable<Cheep> Cheeps { get; set; }
    public required ICollection<Author> Followers { get; set; }
    public required ICollection<Author> Following { get; set; }
    public ICollection<Like> Likes {get; set; }
}
