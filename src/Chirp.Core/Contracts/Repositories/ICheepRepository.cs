using FluentValidation.Results;

namespace Chirp.Core;

public interface ICheepRepository {
    Task<List<CheepDto>> GetCheeps(int page, string? author = null, string? userEmail = null);

    Task<ValidationResult> AddCheep(string message, string authorName, string email);

    Task LikeCheep(string userEmail, string cheepId, bool like);

    Task RemoveLike(string userEmail, string cheepId);
}