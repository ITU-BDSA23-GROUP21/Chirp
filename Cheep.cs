public class Cheep{
    string message;
    string author;
    DateTimeOffset messageTime;

    //Used when creating a new Cheep and writing to CSV file
    public Cheep(string author, string message, DateTimeOffset timeOfMessage){

        this.author = author;
        this.message = message;
        this.messageTime = timeOfMessage;
    }

    //Used when reading Cheep from CSV file
    public Cheep(string author, string message, string unixTime){
        this.author = author;
        this.message = message;
        this.messageTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(unixTime));
    }


    public string ToCsvString(){
        return $"{author},{message},{messageTime.ToUnixTimeSeconds()}";
    }

    public override string ToString(){
        return $"{author} @ {messageTime:dd/mm/yy HH:mm:ss}: {message}";
    }
}