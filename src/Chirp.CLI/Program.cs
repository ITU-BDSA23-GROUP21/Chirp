using System.CommandLine;
using Chirp.CLI.Services.Interfaces;

public class Program
{
    static IChirpHttpClient HttpClient = new ChirpHttpClient();

    private static async Task<int> Main(string[] args)
    {
        // Maybe move this to startup class?
        // Should this happen inside CommandLine Invoke method?
        // HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        // builder.Services.AddSingleton<IChirpHttpClient, ChirpHttpClient>();

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
        var cheeps = await HttpClient.GetCheeps();
        UserInterface.PrintCheeps(cheeps);
    }

    public static async Task AddCheep(string message)
    {
        await AddCheep(message, Environment.UserName);
    }

    public static async Task AddCheep(string message, string userName)
    {
        await HttpClient.PostCheep(message, userName);
    }
}
