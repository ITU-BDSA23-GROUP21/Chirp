using Chirp.Shared;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var cheepHandler = new CheepHandler();

// Fetch all cheeps and return as JSON    
app.MapGet("/cheeps", (int amount) => cheepHandler.GetCheeps(amount));

// Post a cheep into the database
app.MapPost("/cheep", (Cheep cheep) => cheepHandler.AddCheep(cheep.Message, cheep.Author));

app.Run();