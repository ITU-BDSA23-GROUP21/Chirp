using Chirp.Core;
public interface ICheepService {
    public Task<List<CheepDto>> GetCheeps(int page = 1);
    public Task<List<CheepDto>> GetCheepsFromAuthor(string author, int page = 1);
    public Task<FluentValidation.Results.ValidationResult> AddCheep(string message, string authorName);
}

public class CheepService : ICheepService {
    private readonly ICheepRepository _cheepRepository;

    public CheepService(ICheepRepository cheepRepository) => _cheepRepository = cheepRepository;

    public Task<List<CheepDto>> GetCheeps(int page) {
        return _cheepRepository.GetCheeps(page);
    }

    public Task<List<CheepDto>> GetCheepsFromAuthor(string author, int page) {
        // filter by the provided author name
        return _cheepRepository.GetCheeps(page, author);
    }

    //validationresult not same type as in cheepRepository?? casting as quick fix
    public Task<FluentValidation.Results.ValidationResult> AddCheep(string message, string authorName) {
        return (Task<FluentValidation.Results.ValidationResult>)_cheepRepository.AddCheep(message, authorName);
    }
}
