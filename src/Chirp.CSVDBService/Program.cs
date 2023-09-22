using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/cheeps", () => {


    Console.WriteLine("GET");
    //TODO
    //Read all cheeps from database as JSON objects
    //Return a list of cheeps to print
    // Create an HTTP client object
    /* var baseURL = "http://localhost:5193";
    using HttpClient client = new();
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.BaseAddress = new Uri(baseURL);

    // Send an asynchronous HTTP GET request and automatically construct a Cheep object from the
    // JSON object in the body of the response
    var cheep = client.GetFromJsonAsync<Cheep>("cheeps"); */
});

app.MapPost("/cheep", (Cheep cheep) => {

    Console.WriteLine("Post");
    // TODO
    // Post a cheep into the database as a JSON object
});

app.Run();





public record Cheep(string Author, string Message, long Timestamp);
