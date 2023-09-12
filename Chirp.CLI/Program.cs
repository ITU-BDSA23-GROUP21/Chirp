using System.CommandLine;
using System.Text.RegularExpressions;
// using SimpleDB;


// IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

internal sealed class Program {
    private static async Task<int> Main(string[] args) {
        // Workaround for CLI not printing help message if no arguments are passed
        // Inspired by https://stackoverflow.com/a/75734131
        if (args.Length == 0) {
            args = new[] { "--help" };
        }

        // Define available options for CLI input
        var readOption = new Option<bool?>(
            name: "read",
            description: "Read chirps from the database"
            ) {
            // Disallow typing a value for "read", as cli library crashes if parsing to boolean fails
            Arity = ArgumentArity.Zero
        };

        var cheepOption = new Option<string>(
            name: "cheep",
            description: "Send a new cheep"
        );
        var rootCommand = new RootCommand("Welcome to Cheep. Get Chirping.");
        rootCommand.AddOption(cheepOption);
        rootCommand.AddOption(readOption);

        // Define handling of CLI input
        rootCommand.SetHandler(HandleCommands, readOption, cheepOption);

        // Invoke handler with input args
        return await rootCommand.InvokeAsync(args);
    }

    private static void HandleCommands(bool? read, string cheep) {
        // You can currently read and cheep at the same time. Is this intended?
        if (read == true) {
            var cheeps = ReadFile();
            UserInterface.printCheeps(cheeps); // Using static funtion from static class UserInterface
        }

        if (!string.IsNullOrEmpty(cheep)) {
            AddCheep(cheep);
        }
    }

    static private List<Cheep> ReadFile() {
        List<Cheep> cheeps = new();
        try {
            using var sr = new StreamReader("chirp_cli_db.csv");
            sr.ReadLine(); // Skip first line. CSV file format is hardcoded in fileReader
            while (!sr.EndOfStream) {
                // Regex adapted from https://stackoverflow.com/questions/3507498/reading-csv-files-using-c-sharp/
                var line = sr.ReadLine();
                var CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                // Separating columns to array
                string[] splitLine = CSVParser.Split(line);
                splitLine[1] = splitLine[1].Trim('"');

                // Create cheep from splitLine, and add to cheep list 
                cheeps.Add(new Cheep(splitLine[0], splitLine[1], splitLine[2]));
            }
            return cheeps;
        }
        catch (Exception) {
            throw;
        }
    }

    static private void AddCheep(string message) {
        DateTimeOffset dto = DateTimeOffset.Now;
        Cheep cheep = new Cheep(Environment.UserName, message, dto);
        string cheepString = cheep.ToCsvString();
        try {
            using (StreamWriter writer = new StreamWriter("chirp_cli_db.csv", true)) {
                writer.WriteLine(cheepString);
            }
        }
        catch (Exception) {
            throw;
        }

    }
}
//test
