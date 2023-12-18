using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;
public class ChirpContext : DbContext {

    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Like> Likes { get; set; }

    public ChirpContext(DbContextOptions options) : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // Property constraints
        modelBuilder.Entity<Cheep>().Property(c => c.Text).HasMaxLength(160);
        modelBuilder.Entity<Author>().Property(a => a.Email).HasMaxLength(60);

        // Relationships
        modelBuilder.Entity<Like>().HasKey(l => new { l.CheepId, l.AuthorId });

        // Override default delete behaviour, since it caused an error due to multiple cascade paths
        // This means we manually have to delete likes beforehand if we delete an Author
        modelBuilder.Entity<Like>()
            .HasOne(l => l.Author)
            .WithMany(a => a.Likes)
            .HasForeignKey(l => l.AuthorId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }

}
