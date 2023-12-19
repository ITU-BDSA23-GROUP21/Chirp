using FluentValidation.Results;

namespace Chirp.Core;

public interface ICheepRepository {
    Task<List<CheepDto>> GetCheeps(int page, IEnumerable<string> authors, string? userEmail = null);

    Task<ValidationResult> AddCheep(string message, string authorName);

    Task LikeCheep(string userEmail, string cheepId, bool value);

    Task RemoveLike(string userEmail, string cheepId);
}