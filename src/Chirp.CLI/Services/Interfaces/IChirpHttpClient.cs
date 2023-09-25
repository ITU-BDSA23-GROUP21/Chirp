namespace Chirp.CLI.Services.Interfaces {
    public interface IChirpHttpClient {
        Task<IEnumerable<Cheep>?> GetCheeps();

        Task PostCheep(string message, string author);
    }

}