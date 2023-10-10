﻿using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;
public class ChirpContext : DbContext {

    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }

    public string DbPath { get; }

    public ChirpContext() {
        // False warning, since we check for null
        DbPath = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CHIRPDBPATH"))
            ? Path.GetTempPath() + "chirp.db"
            : Environment.GetEnvironmentVariable("CHIRPDBPATH");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

}
