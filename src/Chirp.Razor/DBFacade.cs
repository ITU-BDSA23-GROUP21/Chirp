using Microsoft.Data.Sqlite;
using System.Data;

public interface IDBFacade
{
    public List<CheepViewModel> GetCheeps(string? author = null);
}

public class DBFacade : IDBFacade
{
    string sqlDBFilePath = "../../data/chirp.db";
    public List<CheepViewModel> GetCheeps(string? author = null)
    {
        string query = @"
            SELECT user.username as username, message.text as message, message.pub_date as timestamp
            FROM message
            INNER JOIN user
            ON user.user_id = message.author_id";
        
        if (String.IsNullOrEmpty(author)) {
            query += ";";
        } else {
            query += "WHERE message.author_id = ($name);";
        }

        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = query;
            if (!String.IsNullOrEmpty(author)) {
                command.Parameters.AddWithValue("$name", author);
            }

            using var reader = command.ExecuteReader();
            var retVal = new List<CheepViewModel>();
            while (reader.Read())
            {
                var msg = reader.GetString("message");
                var timeStamp = reader.GetInt64("timestamp");
                var username = reader.GetString("username");

                retVal.Add(new CheepViewModel(username, msg, UnixTimeStampToDateTimeString(timeStamp)));
            }
            
            return retVal;
        }
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}