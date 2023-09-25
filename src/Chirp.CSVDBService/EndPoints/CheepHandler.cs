using Chirp.Shared;
using SimpleDB;

public class CheepHandler {
    private readonly IDatabaseRepository<Cheep> CsvDb = CSVDatabase<Cheep>.Instance;

    public void AddCheep(string message, string author) {
        // Cheeps are stored using server time.
        // This could be solved by sending time from client, or preferable sending timezone, so server can handle it
        DateTimeOffset dto = DateTimeOffset.Now;
        CsvDb.Store(new Cheep(author, message, dto.ToUnixTimeSeconds()));
    }

    public IEnumerable<Cheep> GetCheeps(int amount) {
        return CsvDb.Read(amount);
    }
}