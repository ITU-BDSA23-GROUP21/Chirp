using FluentValidation.Results;

namespace Chirp.Core;

public interface ICheepRepository {
    Task<List<CheepDto>> GetCheeps(int page, string? author = null);

    Task<ValidationResult> AddCheep(string message, string authorName, string email);

    Task<bool> IsFollowing(string follower, string following);

    Task<bool> Follow(string follower, string following);

    Task<bool> UnFollow(string follower, string following);


}