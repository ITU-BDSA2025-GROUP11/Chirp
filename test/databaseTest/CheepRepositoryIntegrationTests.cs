using Xunit;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Chirp.Core.DomainModel;
using System.Linq;

namespace DatabaseTest;

public class CheepRepositoryIntegrationTests : IDisposable
{
    private readonly ChirpDbContext _context;
    private readonly CheepRepository _repo;

    public CheepRepositoryIntegrationTests()
    {
        // Setup in-memory SQLite DB
        var options = new DbContextOptionsBuilder<ChirpDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        _context = new ChirpDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();

        _repo = new CheepRepository(_context, new LoggerFactory());
    }

    [Fact]
    public void Add_And_Retrieve_Cheep()
    {
        _repo.CreateUser();
        _repo.PostCheep("Joakim er faktisk pænt handsome OG har rigtig god humor");

        var cheeps = _repo.GetCheeps();

        Assert.Single(cheeps);
        Assert.Equal("Joakim er faktisk pænt handsome OG har rigtig god humor", cheeps[0].Text);
    }

    [Fact]
    public void PostCheep_ShouldLinkToCorrectAuthor()
    {
        _repo.CreateUser();
        _repo.PostCheep("Integration test cheep");

        var cheep = _context.Cheeps.Include(c => c.Author).First();

        Assert.Equal(Environment.UserName, cheep.Author.Name);
        Assert.Equal("Integration test cheep", cheep.Text);
    }

    [Fact]
    public void PostCheep_WithoutUser_ShouldNotThrow()
    {
        _context.Authors.RemoveRange(_context.Authors);
        _context.SaveChanges();

        var ex = Record.Exception(() => _repo.PostCheep("This should still work"));

        Assert.Null(ex);
    }
    
    public void Dispose()
    {
        _context.Database.CloseConnection();
        _context.Dispose();
    }
}