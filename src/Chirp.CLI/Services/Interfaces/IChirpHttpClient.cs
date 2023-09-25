using Chirp.Shared;

namespace Chirp.CLI.Services.Interfaces {
    public interface IChirpHttpClient {
        Task<IEnumerable<Cheep>?> GetCheeps(int amount);

        Task PostCheep(string message, string author);
    }

}