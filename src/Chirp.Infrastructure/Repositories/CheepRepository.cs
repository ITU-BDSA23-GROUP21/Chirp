using Chirp.Core;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository {
    private readonly ChirpContext _dbContext;

    public CheepRepository(ChirpContext dbContext) => _dbContext = dbContext;

    public Task<List<CheepDto>> GetCheeps(int page, string? author = null) {
        return _dbContext.Cheeps
            .Where( cheep => cheep.Author.Name == author || author == null)
            .OrderByDescending(cheep => cheep.TimeStamp)
            .Skip( 32 * (page - 1))
            .Take(32)
            .Select( cheep => new CheepDto(cheep.Author.Name, cheep.Text, cheep.TimeStamp.ToString("MM/dd/yy H:mm:ss")))
            .ToListAsync();
    }
}