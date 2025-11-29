using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace PagesTest;

public class LikeDislikeTests
{
    public required CheepRepository Repository;
    public required ChirpDbContext Context;
    public required SqliteConnection Connection;

    private void Before()
    {
        Connection = new SqliteConnection("Filename=:memory:");
        Connection.Open();

        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(Connection);

        Context = new ChirpDbContext(builder.Options);
        Context.Database.EnsureCreated();

        Repository = new CheepRepository(Context, NullLoggerFactory.Instance);
    }

    [Fact]
    public async Task LikeAddsLikeToCheep()
    {
        
    }
}