using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;
public class ChirpContext : DbContext {

    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }

    public ChirpContext(DbContextOptions options) : base(options) { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Cheep>().Property(c => c.Text).HasMaxLength(160);
        modelBuilder.Entity<Author>().Property(a => a.Email).HasMaxLength(60);
    }

}
