namespace Chirp.Core;

public interface ICheepRepository {
    Task<List<CheepDto>> GetCheeps(int page, string? author = null);
}