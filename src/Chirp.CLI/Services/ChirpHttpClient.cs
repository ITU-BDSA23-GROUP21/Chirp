using Chirp.Shared;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

// Singleton class, that exposes methods for communicating with Chirp API
public class ChirpHttpClient {
    private static ChirpHttpClient? instance;
    private readonly HttpClient HttpClient;

    private ChirpHttpClient() {
        var uri = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? "http://localhost:5193" : "https://bdsagroup21chirpremotedb.azurewebsites.net";
        HttpClient = new() {
            BaseAddress = new Uri(uri),
        };
    }
    public static ChirpHttpClient Instance {
        get {
            if (instance == null) {
                instance = new ChirpHttpClient();
            }
            return instance;
        }
    }

    public Task<IEnumerable<Cheep>?> GetCheeps(int amount) {
        return HttpClient.GetFromJsonAsync<IEnumerable<Cheep>>(
            $"/cheeps?amount={amount}"
        );
    }

    public async Task PostCheep(string message, string author) {
        using StringContent jsonContent = new(
            JsonSerializer.Serialize(new {
                Author = Environment.UserName,
                Message = message,
            }),
            Encoding.UTF8,
            "application/json");

        using HttpResponseMessage response = await HttpClient.PostAsync(
            "cheep",
            jsonContent);

        response.EnsureSuccessStatusCode();

        // TODO: Error handling?
    }
}
