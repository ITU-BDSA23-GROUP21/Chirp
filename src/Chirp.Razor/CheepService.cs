using Chirp.Core;
using FluentValidation.Results;
using Microsoft.Graph.Models;
using System.Collections;

namespace Chirp.Razor;

public interface ICheepService {
    public Task<List<CheepDto>> GetCheeps(int page = 1, string? userEmail = null);
    public Task<List<CheepDto>> GetCheepsFromAuthor(string? author, int page = 1, string? userEmail = null);
    public Task<ValidationResult> AddCheep(string message, string authorName, string email);
    public Task<List<CheepDto>> GetCheepsFromAuthors(IEnumerable<string?> authors, int page = 1, string? userEmail = null);
    public Task LikeCheep(string userEmail, string cheepId, bool like);
    public Task RemoveLike(string userEmail, string cheepId);
}

public class CheepService : ICheepService {
    private readonly ICheepRepository _cheepRepository;

    public CheepService(ICheepRepository cheepRepository) => _cheepRepository = cheepRepository;

    public Task<List<CheepDto>> GetCheeps(int page, string? userEmail = null) {
        if (page <= 0) page = 1;
        return _cheepRepository.GetCheeps(page, null, userEmail);
    }

    public Task<List<CheepDto>> GetCheepsFromAuthor(string? author, int page, string? userEmail = null) {
        // filter by the provided author name
        if (author == null) {
            throw new ArgumentNullException(nameof(author));
        }
        
        return _cheepRepository.GetCheeps(page, new List<string?> {author}, userEmail);
    }

    public async Task<List<CheepDto>> GetCheepsFromAuthors(IEnumerable<string?> authors, int page, string? userEmail = null) {
        if (!authors.Any()) {
            // Should we just get all cheeps instead or throwing an exception?
            throw new ArgumentException("No authors supplied", nameof(authors));
        }

        List<CheepDto> cheepDtos = new List<CheepDto>();

            // Should we refactor repo to handle list of authors, so we dont have to open new connection for each author?
            List<CheepDto> cheeps = await _cheepRepository.GetCheeps(page, authors, userEmail);
            // foreach (var cheep in cheeps) {
            //     cheepDtos.Add(cheep);
            // }
        //List<CheepDto> returnList = cheepDtos.ToList();

        return cheeps;
    }

    public async Task<ValidationResult> AddCheep(string message, string authorName, string email) {
        return await _cheepRepository.AddCheep(message, authorName, email);
    }

    public async Task LikeCheep(string userEmail, string cheepId, bool like) {
        await _cheepRepository.LikeCheep(userEmail, cheepId, like);
    }

    public async Task RemoveLike(string userEmail, string cheepId) {
        await _cheepRepository.RemoveLike(userEmail, cheepId);
    }

}
