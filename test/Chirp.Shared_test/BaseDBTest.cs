using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Chirp.Shared_test;
public class BaseDBTest
{

    private readonly DbConnection _connection;
    private readonly DbContextOptions<ChirpContext> _contextOptions;
    public BaseDBTest() {
        // Adapted from example at https://learn.microsoft.com/en-us/ef/core/testing/testing-without-the-database#sqlite-in-memory
        
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<ChirpContext>()
            .UseSqlite(_connection)
            .Options;

        // Create the schema and seed some data
        using var context = new ChirpContext(_contextOptions);

        if (context.Database.EnsureCreated())
        {
            DbInitializer.SeedDatabase(context);

//             using var viewCommand = context.Database.GetDbConnection().CreateCommand();
//             viewCommand.CommandText = @"
// CREATE VIEW AllResources AS
// SELECT Url
// FROM Blogs;";
//             viewCommand.ExecuteNonQuery();
        }

        // context.AddRange(
        //     new  { Author = "Author1", Message = "Chirp message 1", Timestamp = "000" },
        //     new  { Author = "Author2", Message = "Chirp message 2", Timestamp = "000"});
        // context.SaveChanges();
    }

    protected ChirpContext CreateContext() => new(_contextOptions);

    public void Dispose() => _connection.Dispose();
}
