using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;
public class ChirpContext : DbContext {

    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }

    public string DbPath { get; }

    public ChirpContext() {
        // False warning, since we check for null
        DbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH") 
            ?? Path.Combine(Path.GetTempPath(), "chirp.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

}
