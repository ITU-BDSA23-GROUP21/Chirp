using Chirp.Core;
using FluentValidation.Results;
public interface ICheepService {
    public Task<List<CheepDto>> GetCheeps(int page = 1);
    public Task<List<CheepDto>> GetCheepsFromAuthor(string? author, int page = 1);
    public Task<ValidationResult> AddCheep(string message, string authorName, string email);
}

public class CheepService : ICheepService {
    private readonly ICheepRepository _cheepRepository;

    public CheepService(ICheepRepository cheepRepository) => _cheepRepository = cheepRepository;

    public Task<List<CheepDto>> GetCheeps(int page) {
        if (page <= 0) page = 1;
        return _cheepRepository.GetCheeps(page);
    }

    public Task<List<CheepDto>> GetCheepsFromAuthor(string? author, int page) {
        // filter by the provided author name
        if (author == null) {
            throw new ArgumentNullException(nameof(author));
        }
        return _cheepRepository.GetCheeps(page, author);
    }

    //validationresult not same type as in cheepRepository?? casting as quick fix
    public async Task<ValidationResult> AddCheep(string message, string authorName, string email) {
        return await _cheepRepository.AddCheep(message, authorName, email);
    }
}
