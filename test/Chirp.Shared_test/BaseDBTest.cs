using Chirp.Infrastructure;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using Testcontainers.PostgreSql;
using Xunit;

namespace Chirp.Shared_test;
public abstract class BaseDBTest : IAsyncLifetime {
    // Suppressing warning, as InitializeAsync ensures that _context will have a non-null value.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ChirpContext _context;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private readonly DockerContainer _container
        = Environment.GetEnvironmentVariable("SERVER") == "POSTGRES" ? new PostgreSqlBuilder().Build() : new MsSqlBuilder().Build();

    public async Task InitializeAsync() {
        await _container.StartAsync();
        // Casting as a quick fix for type issues. Should be safe, as _container is readonly and only instantiated as a database container
        _context = GetContext(_container as IDatabaseContainer);
        await _context.Database.EnsureCreatedAsync();
        DbInitializer.SeedDatabase(_context);
    }

    public Task DisposeAsync()
        => _container.DisposeAsync().AsTask();

    public static ChirpContext GetContext(IDatabaseContainer? container)
        => Environment.GetEnvironmentVariable("SERVER") == "POSTGRES" ?
            new(new DbContextOptionsBuilder().UseNpgsql(container?.GetConnectionString()).Options) :
            new(new DbContextOptionsBuilder().UseSqlServer(container?.GetConnectionString()).Options);
}
