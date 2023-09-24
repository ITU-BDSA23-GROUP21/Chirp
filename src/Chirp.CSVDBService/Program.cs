using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using SimpleDB;

using HttpClient client = new();
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
IDatabaseRepository<Cheep>? CSVdb = CSVDatabase<Cheep>.Instance;

List<Cheep> list = new List<Cheep> {
        new Cheep("Mig", "Hej", 1),
        new Cheep("Dig", "Dav", 2)
    };

// TODO
// Fetch all cheeps and return as JSON strings    
app.MapGet("/cheeps", () => CSVdb.Read());

// TODO
// Post a cheep into the database as a JSON object
app.MapPost("/cheep", (Cheep cheep) => CSVdb.Store(cheep));

app.Run();

internal record Cheep(string Author, string Message, long Timestamp);
