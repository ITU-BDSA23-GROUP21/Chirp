public record Cheep{
    string author;
    string message;
    long timestamp;

    //Used when creating a new Cheep and writing to CSV file
    public Cheep(string author, string message, long timestamp){

        this.author = author;
        this.message = message;
        this.timestamp = timestamp;
    }

    //Used when reading Cheep from CSV file
    public Cheep(string author, string message, string timestamp){
        this.author = author;
        this.message = message;
        this.timestamp = long.Parse(timestamp);
    }


    public string ToCsvString(){
        return $"{author},\"{message}\",{timestamp}";
    }

    private DateTimeOffset LongToLocalTime() {
        return DateTimeOffset.FromUnixTimeSeconds(this.timestamp);
    }

    public override string ToString(){
        return $"{author} @ {LongToLocalTime():dd/mm/yy HH:mm:ss}: {message}";
    }
}
