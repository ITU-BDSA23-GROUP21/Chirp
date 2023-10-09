namespace Chirp.Core;

public interface ICheepRepository {
    IEnumerable<CheepDto> GetCheeps(int page, string? author = null);
}