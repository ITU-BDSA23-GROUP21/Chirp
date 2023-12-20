using Chirp.Infrastructure;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using Testcontainers.PostgreSql;
using Xunit;

namespace Chirp.Shared_test;
public abstract class BaseDBTest : IAsyncLifetime
{
    protected ChirpContext _context;
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
