using Chirp.Core;

public interface ICheepService {
    public Task<List<CheepDto>> GetCheeps(int page = 1);
    public Task<List<CheepDto>> GetCheepsFromAuthor(string author, int page = 1);
}

public class CheepService : ICheepService {
    private readonly ICheepRepository cheepRepository;

    public CheepService(ICheepRepository _cheepRepository) => cheepRepository = _cheepRepository;

    public Task<List<CheepDto>> GetCheeps(int page) {
        return cheepRepository.GetCheeps(page);
    }

    public Task<List<CheepDto>> GetCheepsFromAuthor(string author, int page) {
        // filter by the provided author name
        return cheepRepository.GetCheeps(page, author);
    }
}
