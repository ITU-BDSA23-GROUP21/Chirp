using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;
public class ChirpContext : DbContext {

    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Cheep>().Property(c => c.Text).HasMaxLength(160);
        modelBuilder.Entity<Author>().Property(a => a.Email).HasMaxLength(60);
    }

}
