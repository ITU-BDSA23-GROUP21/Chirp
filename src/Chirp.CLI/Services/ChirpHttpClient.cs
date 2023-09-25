using Chirp.CLI.Services.Interfaces;
using Chirp.Shared;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

// This should only be instantiated once. Singleton or DI?
public class ChirpHttpClient : IChirpHttpClient {
    private readonly HttpClient HttpClient;

    public ChirpHttpClient() {
        HttpClient = new() {
            BaseAddress = new Uri("http://localhost:5193"),
        };
    }

    public Task<IEnumerable<Cheep>?> GetCheeps() {
        return HttpClient.GetFromJsonAsync<IEnumerable<Cheep>>(
            "/cheeps"
        );
    }

    public async Task PostCheep(string message, string author) {
        // Not implemented yet
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