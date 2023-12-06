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
        modelBuilder.Entity<Likes>().HasKey(l => new { l.CheepId, l.AuthorId });

        // Override default delete behaviour, since it will cause multiple cascade paths
        // NB: This means we manually have to delete likes with null AuthorId after deleting an Author
        modelBuilder.Entity<Likes>()
            .HasOne(l => l.Author)
            .WithMany(a => a.Likes)
            .HasForeignKey(l => l.AuthorId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }

}
