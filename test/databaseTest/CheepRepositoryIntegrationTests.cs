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
        var options = new DbContextOptionsBuilder<ChirpDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        _context = new ChirpDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated(); 

        _repo = new CheepRepository(_context, new LoggerFactory());
    }
    //shows that we can add and retrive cheeps from the repo
    [Fact]
    public void Add_And_Retrieve_Cheep()
    {
        _repo.CreateUser();
        _repo.PostCheep("Joakim er faktisk pænt handsome OG har rigtig god humor");

        var cheeps = _repo.GetCheeps();

        Assert.Single(cheeps);
        Assert.Equal("Joakim er faktisk pænt handsome OG har rigtig god humor", cheeps[0].Text);
    }

   //Checks that our cheeps get assigned to the corrct author
    [Fact]
    public void Cheep_Belongs_To_Correct_Author()
    {
        _repo.CreateUser();
        _repo.PostCheep("Resten af gruppen er også pænt cool");

        var cheep = _context.Cheeps.Include(c => c.Author).First();

        Assert.Equal(Environment.UserName, cheep.Author.Name);
        Assert.Equal("Resten af gruppen er også pænt cool", cheep.Text);
    }

    //chekcs that we handle a missing author
    [Fact]
    public void Missing_Author()
    {
        _context.Authors.RemoveRange(_context.Authors);
        _context.SaveChanges();

        var ex = Record.Exception(() => _repo.PostCheep("Plz no crash test"));

        Assert.Null(ex);
    }

   //checks that we only get cheeps from the desired author 
    [Fact]
    public void Get_Cheeps_From_Author()
    {
        _repo.CreateUser();
        _repo.PostCheep("Joakim’s cheep 1");
        _repo.PostCheep("Joakim’s cheep 2");
        
        var otherAuthor = new Author
        {
            Name = "SomeoneElse",
            Email = "someone@mail.com",
            Cheeps = new List<Cheep>
            {
                new Cheep { Text = "Other’s cheep", TimeStamp = DateTime.Now }
            }
        };
        _context.Authors.Add(otherAuthor);
        _context.SaveChanges();
        
        var cheeps = _repo.GetCheeps(author: Environment.UserName);
        
        Assert.All(cheeps, c => Assert.Equal(Environment.UserName, c.Author.Username));
        Assert.DoesNotContain(cheeps, c => c.Author.Username == "SomeoneElse");
    }
   
    public void Dispose()
    {
        _context.Database.CloseConnection();
        _context.Dispose();
    }
}
