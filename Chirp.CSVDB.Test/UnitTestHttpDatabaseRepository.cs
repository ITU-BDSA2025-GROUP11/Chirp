using Xunit;
using System.Linq;

namespace Chirp.CSVDB.Test;

public class IntegrationTestHttpRepository
{
    private readonly HttpDatabaseRepository _repo;

    public IntegrationTestHttpRepository()
    {
        _repo = new HttpDatabaseRepository("http://localhost:5000");
    }

    [Fact]
    //Needs to be connected to local host to suceed.
    public void CanStoreAndReadCheep()
    {
        var cheep = new Cheep("HttpUser", "Test", DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        _repo.Store(cheep);
        var cheeps = _repo.Read().ToList();

        Assert.Contains(cheeps, c => c.Author == "HttpUser" && c.Message == "Test");
    }
}