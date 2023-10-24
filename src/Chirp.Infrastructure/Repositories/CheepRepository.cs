using Chirp.Core;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository {
    private readonly ChirpContext _dbContext;

    public CheepRepository(ChirpContext dbContext) =>
        _dbContext = dbContext;

    public Task<List<CheepDto>> GetCheeps(int page, string? author = null) {
        return _dbContext.Cheeps
            .Where(cheep => cheep.Author.Name == author || author == null)
            .OrderByDescending(cheep => cheep.TimeStamp)
            .Skip(32 * (page - 1))
            .Take(32)
            .Select(cheep => new CheepDto(cheep.Author.Name, cheep.Text, cheep.TimeStamp.ToString("MM/dd/yy H:mm:ss")))
            .ToListAsync();
    }

    public async Task AddCheep(string message, string authorName) {
        // We have no way of knowing the correct email if author does not exist already
        var author = await _dbContext.Authors.Where(author => author.Name == authorName).FirstOrDefaultAsync();

        if (author == null) {
            author = new Author() {
                Id = Guid.NewGuid(),
                Name = authorName,
                Email = "thisisalegitemail@email.email",
                Cheeps = new List<Cheep>()
            };

            await _dbContext.Authors.AddAsync(author);
        }

        await _dbContext.Cheeps.AddAsync(new Cheep() { Id = Guid.NewGuid(), AuthorId = author.Id, Author = author, Text = message });
        _dbContext.SaveChanges();
    }
}
