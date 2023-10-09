using Chirp.Core;

namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository {
    public IEnumerable<CheepDto> GetCheeps(int page, string? author = null) {
        throw new NotImplementedException();
    }
}