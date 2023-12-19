using Chirp.Core;
using FluentValidation.Results;
using Microsoft.Graph.Models;
using System.Collections;

namespace Chirp.Razor;

public interface ICheepService {
    public Task<List<CheepDto>> GetCheeps(int page = 1, string? userEmail = null);
    public Task<List<CheepDto>> GetCheepsFromAuthors(IEnumerable<string> authors, int page = 1, string? userEmail = null);
    public Task<ValidationResult> AddCheep(string message, string authorName);
    public Task LikeCheep(string userEmail, string cheepId, bool like);
    public Task RemoveLike(string userEmail, string cheepId);
}

public class CheepService : ICheepService {
    private readonly ICheepRepository _cheepRepository;

    public CheepService(ICheepRepository cheepRepository) => _cheepRepository = cheepRepository;

    public Task<List<CheepDto>> GetCheeps(int page, string? userEmail = null) {
        return GetCheepsFromAuthors(Enumerable.Empty<string>(), page, userEmail);
    }

    public Task<List<CheepDto>> GetCheepsFromAuthors(IEnumerable<string> authors, int page, string? userEmail = null) {
        if (page <= 0) page = 1;
        return _cheepRepository.GetCheeps(page, authors, userEmail);
    }

    public async Task<ValidationResult> AddCheep(string message, string authorName) {
        return await _cheepRepository.AddCheep(message, authorName);
    }

    public async Task LikeCheep(string userEmail, string cheepId, bool like) {
        await _cheepRepository.LikeCheep(userEmail, cheepId, like);
    }

    public async Task RemoveLike(string userEmail, string cheepId) {
        await _cheepRepository.RemoveLike(userEmail, cheepId);
    }

}
