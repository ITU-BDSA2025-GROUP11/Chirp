using Xunit;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Chirp.Core.DomainModel;
using Microsoft.Extensions.Logging;

namespace DatabaseTest;

public class CheepRepositoryIntegrationTests
{
    //Tester om vi kan vi kan tilføje og hente Cheeps fra et datbase.
    [Fact]
    public void AddAndRetrieveCheep()
    {
        var options = new DbContextOptionsBuilder<ChirpDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        using var context = new ChirpDbContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        var repo = new CheepRepository(context, new LoggerFactory());
        
        repo.CreateUser();
        repo.PostCheep("Joakim er faktisk pænt handsome OG har rigtig god humor");

        var cheeps = repo.GetCheeps();
        
        Assert.Single(cheeps);
        Assert.Equal("Joakim er faktisk pænt handsome OG har rigtig god humor", cheeps[0].Text);
    }
}

