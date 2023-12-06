using System.ComponentModel.DataAnnotations.Schema;

namespace Chirp.Infrastructure;

public class Like {
    public Guid CheepId {get; set; }
    public Guid AuthorId {get; set; }

    public Cheep Cheep {get; set;}
    public Author Author {get; set;}
    public required bool Liked {get; set; }
}
