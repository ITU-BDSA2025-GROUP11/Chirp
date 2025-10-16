using Xunit;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Chirp.Core.DomainModel;
using Microsoft.Extensions.Logging;

namespace DatabaseTest;

public class CheepRepositoryIntegrationTests
{
    [Fact]
    public void Can_Add_And_Retrieve_Cheep()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ChirpDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        using var context = new ChirpDbContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        var repo = new CheepRepository(context, new LoggerFactory());

        // Act
        repo.CreateUser();
        repo.PostCheep("Joakim er faktisk pænt handsome OG meget morsom!");

        var cheeps = repo.GetCheeps();

        // Assert
        Assert.Single(cheeps);
        Assert.Equal("Joakim er faktisk pænt handsome OG meget morsom!", cheeps[0].Text);
    }
}