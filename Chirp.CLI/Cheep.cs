using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

public record struct Cheep() {

    //Used when creating a new Cheep and writing to CSV file
    public Cheep(string Author, string Message, long Timestamp)
    {
        this.Author = Author;
        this.Message = Message;
        this.Timestamp = Timestamp;
    }

    //Used when reading Cheep from CSV file
    // public Cheep(string author, string message, string timestamp){
    //     this.Author = author;
    //     this.Message = message;
    //     this.Timestamp = long.Parse(timestamp);
    // }


    public required string Author {get; init;}
    public required string Message {get; init;}
    public required long Timestamp {get; init;}

    public string ToCsvString(){
        return $"{Author},\"{Message}\",{Timestamp}";
    }

    private DateTimeOffset LongToLocalTime() {
        return DateTimeOffset.FromUnixTimeSeconds(this.Timestamp);
    }

    public override string ToString(){
        return $"{Author} @ {LongToLocalTime():dd/mm/yy HH:mm:ss}: {Message}";
    }
}
