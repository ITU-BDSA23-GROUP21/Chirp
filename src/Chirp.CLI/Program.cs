using SimpleDB;
using System.CommandLine;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

public class Program
{
    static IDatabaseRepository<Cheep>? CSVdb;
    static HttpClient? client;

    private static async Task<int> Main(string[] args)
    {
        setDB();
        setClient();

        //Console.WriteLine(client);
        //await GetFromJsonAsync(client);
        // Workaround for CLI not printing help message if no arguments are passed
        // Inspired by https://stackoverflow.com/a/75734131
        if (args.Length == 0)
        {
            args = new[] { "--help" };
        }

        //The rootcommand
        RootCommand rootCommand = new RootCommand("Welcome to Cheep. Get Chirping!");
        rootCommand.SetHandler(() => { Console.WriteLine("Welcome to Cheep. Get Chirping!"); });

        //Subcommand read and arguments
        Command readCommand = new Command("read", "Read cheeps.");
        rootCommand.AddCommand(readCommand);

        Argument<int> amountArgument = new Argument<int>(
            name: "amount",
            description: "Specifies the amount of cheeps to show",
            getDefaultValue: () => 5
        );
        readCommand.AddArgument(amountArgument);
        readCommand.SetHandler(PrintCheeps, amountArgument);

        //Subcommand cheep and arguments
        Command cheepCommand = new Command("cheep", "Cheep a message.");
        rootCommand.AddCommand(cheepCommand);

        Argument<string> messageArgument = new Argument<string>(
            name: "message",
            description: "The message to cheep."
        );
        cheepCommand.AddArgument(messageArgument);
        cheepCommand.SetHandler(AddCheep, messageArgument);

        // Invoke handler with input args
        return await rootCommand.InvokeAsync(args);
    }

    public static async Task PrintCheeps(int amount)
    {
        // You can currently read and cheep at the same time. Is this intended?
        await GetFromJsonAsync(client);

    }

    public static async Task AddCheep(string message)
    {
        DateTimeOffset dto = DateTimeOffset.Now.ToLocalTime();
        Cheep cheep = new(Environment.UserName, message, dto.ToUnixTimeSeconds());

        // Send POST request to DB

        //CSVdb.Store(cheep);
        await PostAsync(client, message);
    }

    public static IDatabaseRepository<Cheep> setDB()
    {
        CSVdb = CSVDatabase<Cheep>.Instance;
        return CSVdb;
    }

    public static HttpClient setClient()
    {
        return client = sharedClient;
    }

    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("http://localhost:5193"),
    };

    // Only works if Cheeps are stored in a list
    static async Task GetFromJsonAsync(HttpClient httpClient)
    {
        var cheeps = await httpClient.GetFromJsonAsync<List<Cheep>>(
            "/cheeps"
        );

        UserInterface.PrintCheeps(cheeps);
    }

    static async Task PostAsync(HttpClient httpClient, string Message)
    {
        DateTimeOffset dto = DateTimeOffset.Now.ToLocalTime();
        using StringContent jsonContent = new(
            JsonSerializer.Serialize(new
            {
                Author = Environment.UserName,
                Message = Message,
                Timestamp = dto.ToUnixTimeSeconds()
            }),
            Encoding.UTF8,
            "application/json");

        using HttpResponseMessage response = await httpClient.PostAsync(
            "cheep",
            jsonContent);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        //Console.WriteLine($"{jsonResponse}"); //  this line creates a "cannot write to a closed writer" exception
    }
}
