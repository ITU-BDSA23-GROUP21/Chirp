using Microsoft.Data.Sqlite;
using Microsoft.Extensions.FileProviders;
using System.Data;
using System.Reflection;

public interface IDBFacade {
    public List<CheepViewModel> GetCheeps(string? author = null);
}

public class DBFacade : IDBFacade {
    string sqlDBFilePath;

    public DBFacade() {
        // False warning, since we check for null
        sqlDBFilePath = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CHIRPDBPATH"))
            ? Path.GetTempPath() + "chirp.db"
            : Environment.GetEnvironmentVariable("CHIRPDBPATH");
        Console.WriteLine(Path.GetTempPath());

        if (!File.Exists(sqlDBFilePath)) {
            var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
            using var reader = embeddedProvider.GetFileInfo("Schema.sql").CreateReadStream();
            using var sr = new StreamReader(reader);
            var query = sr.ReadToEnd();
            ExecuteCommand(query);
        }
    }


    public List<CheepViewModel> GetCheeps(string? author = null) {
        string query = @"
            SELECT user.username as username, message.text as message, message.pub_date as timestamp
            FROM message
            INNER JOIN user
            ON user.user_id = message.author_id";

        if (String.IsNullOrEmpty(author)) {
            query += ";";
        }
        else {
            query += "WHERE message.author_id = ($name);";
        }

        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}")) {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = query;
            if (!String.IsNullOrEmpty(author)) {
                command.Parameters.AddWithValue("$name", author);
            }

            using var reader = command.ExecuteReader();
            var retVal = new List<CheepViewModel>();
            while (reader.Read()) {
                var msg = reader.GetString("message");
                var timeStamp = reader.GetInt64("timestamp");
                var username = reader.GetString("username");

                retVal.Add(new CheepViewModel(username, msg, UnixTimeStampToDateTimeString(timeStamp)));
            }

            return retVal;
        }
    }

    private int ExecuteCommand(string cmd) {
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}")) {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = cmd;

            return command.ExecuteNonQuery();
        }
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp) {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}
