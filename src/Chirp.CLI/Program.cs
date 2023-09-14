using SimpleDB;
using System.CommandLine;


// IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

internal class Program {
    static IDatabaseRepository<Cheep>? CSVdb;

    private static async Task<int> Main(string[] args) {
        CSVdb = new CSVDatabase<Cheep>("../../data/chirp_cli_db.csv");
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
            var cheeps = CSVdb.Read();
            UserInterface.PrintCheeps(cheeps);
        }

        if (!string.IsNullOrEmpty(cheep)) {
            AddCheep(cheep);
        }
    }

    static private void AddCheep(string message) {
        DateTimeOffset dto = DateTimeOffset.Now.ToLocalTime();
        Cheep cheep = new(Environment.UserName, message, dto.ToUnixTimeSeconds());
        CSVdb.Store(cheep);
    }
}
// test