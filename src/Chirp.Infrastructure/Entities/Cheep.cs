namespace Chirp.Infrastructure;

public class Cheep {
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }

    // Why are we expected to store both AuthorId and full Author?
    public Author Author {get; set;}
}
