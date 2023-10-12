using System.ComponentModel.DataAnnotations;

namespace Chirp.Infrastructure;

public class Cheep {
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }

    // Why are we expected to store both AuthorId and full Author?
    public required Author Author {get; set;}
}
