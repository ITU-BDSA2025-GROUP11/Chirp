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

    private string testName = "Test";
    private string testMail = "Author";

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
    public async Task Add_And_Retrieve_Cheep()
    {
        _repo.CreateUser(testName, testMail);
        _repo.PostCheep("Joakim er faktisk pænt handsome OG har rigtig god humor", testName, testMail);

        var cheeps = await _repo.GetCheeps();

        Assert.Single(cheeps);
        Assert.Equal("Joakim er faktisk pænt handsome OG har rigtig god humor", cheeps[0].Text);
    }

   //Checks that our cheeps get assigned to the corrct author
    [Fact]
    public void Cheep_Belongs_To_Correct_Author()
    {
        _repo.CreateUser(testName, testMail);
        _repo.PostCheep("Resten af gruppen er også pænt cool",  testName, testMail);

        var cheep = _context.Cheeps.Include(c => c.Author).First();

        Assert.Equal(testName, cheep.Author.UserName);
        Assert.Equal("Resten af gruppen er også pænt cool", cheep.Text);
    }

    //chekcs that we handle a missing author
    [Fact]
    public async Task Missing_Author()
    {
        _context.Users.RemoveRange(_context.Users);
        _context.SaveChanges();

        var ex =  Record.ExceptionAsync(() => _repo.PostCheep("Plz no crash test",  testName, testMail));

        Assert.Null(ex);
    }

   //checks that we only get cheeps from the desired author 
   [Fact]
   public async Task Get_Cheeps_From_Author()
   {
       _repo.CreateUser(testName, testMail);
       _repo.PostCheep("Joakim’s cheep 1",  testName, testMail);
       _repo.PostCheep("Joakim’s cheep 2",   testName, testMail);
    
       var otherAuthor = new Author
       {
           UserName = "SomeoneElse",
           Email = "someone@mail.com",
           Cheeps = new List<Cheep>()
       };

       var otherCheep = new Cheep
       {
           Text = "Other’s cheep",
           TimeStamp = DateTime.Now,
           Author = otherAuthor
       };

       otherAuthor.Cheeps.Add(otherCheep);
       _context.Users.Add(otherAuthor);
       _context.SaveChanges();
    
       var cheeps = await _repo.GetCheeps(author: testName);
    
       Assert.All(cheeps, c => Assert.Equal(testName, c.Author.Username));
       Assert.DoesNotContain(cheeps, c => c.Author.Username == "SomeoneElse");
   }

   
    public void Dispose()
    {
        _context.Database.CloseConnection();
        _context.Dispose();
    }
}
