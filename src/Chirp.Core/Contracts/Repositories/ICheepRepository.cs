namespace Chirp.Core;
using FluentValidation;

public interface ICheepRepository {
    Task<List<CheepDto>> GetCheeps(int page, string? author = null);

    Task<FluentValidation.Results.ValidationResult> AddCheep(string message, string authorName, string email);
}