using Chirp.Core;
using FluentValidation.Results;

namespace Chirp.Razor;

public interface ICheepService {
    public Task<List<CheepDto>> GetCheeps(int page = 1);
    public Task<List<CheepDto>> GetCheepsFromAuthor(string? author, int page = 1);
    public Task<ValidationResult> AddCheep(string message, string authorName, string email);

    public Task<List<CheepDto>> GetCheepsFromAuthors(IEnumerable<AuthorDto> authors,string authorName, int page = 1);
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

    public async Task<List<CheepDto>> GetCheepsFromAuthors(IEnumerable<AuthorDto> authors,string authorName, int page){
        List<CheepDto> cheepDtos = new List<CheepDto>();        

        foreach (var author in authors){
            List<CheepDto> cheeps = await _cheepRepository.GetCheeps(page, author.Name);
            foreach (var cheep in cheeps){
                cheepDtos.Add(cheep);
            }
        }
        foreach (var cheep in  await _cheepRepository.GetCheeps(page, authorName)){
                cheepDtos.Add(cheep);
            }
        List<CheepDto> returnList = cheepDtos.OrderByDescending(o => o.Timestamp).ToList();
        
        return returnList;
    }

    public async Task<ValidationResult> AddCheep(string message, string authorName, string email) {
        return await _cheepRepository.AddCheep(message, authorName, email);
    }


}
