using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Chirp.Core.DomainModel;
using Chirp.Core.DTO;

namespace DatabaseTest;

public class CheepRepositoryIntegrationTests : IDisposable
{
    private readonly ChirpDbContext _context;
    private readonly CheepRepository _cheepRepo;
    private readonly AuthorRepository _authorRepo;

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

        _cheepRepo = new CheepRepository(_context, new LoggerFactory());
        _authorRepo = new AuthorRepository(_context, new LoggerFactory());
    }
    //shows that we can add and retrive cheeps from the repo
    [Fact]
    public async Task Add_And_Retrieve_Cheep()
    {
        await _authorRepo.CreateUser(testName, testMail);
        await _cheepRepo.PostCheep("Joakim er faktisk pænt handsome OG har rigtig god humor", testName, testMail);

        var cheeps = await _cheepRepo.GetCheeps();

        Assert.Single(cheeps);
        Assert.Equal("Joakim er faktisk pænt handsome OG har rigtig god humor", cheeps[0].Text);
    }

    //Checks that our cheeps get assigned to the corrct author
    [Fact]
    public async Task Cheep_Belongs_To_Correct_Author()
    {
        await _authorRepo.CreateUser(testName, testMail);
        await _cheepRepo.PostCheep("Resten af gruppen er også pænt cool",  testName, testMail);

        var cheep = await _context.Cheeps.Include(c => c.Author).FirstAsync();

        Assert.Equal(testName, cheep.Author.UserName);
        Assert.Equal("Resten af gruppen er også pænt cool", cheep.Text);
    }

    //chekcs that we handle a missing author
    [Fact]
    public async Task Missing_Author()
    {
        _context.Users.RemoveRange(_context.Users);
        _context.SaveChanges();

        await Assert.ThrowsAsync<ArgumentException>(() => _cheepRepo.PostCheep("Plz no crash test", testName, testMail));
    }

    //checks that we only get cheeps from the desired author 
    [Fact]
    public async Task Get_Cheeps_From_Author()
    {
        await _authorRepo.CreateUser(testName, testMail);
        await _cheepRepo.PostCheep("Joakim’s cheep 1",  testName, testMail);
        await _cheepRepo.PostCheep("Joakim’s cheep 2",   testName, testMail);
    
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
        await _context.SaveChangesAsync();
    
        var cheeps = await _cheepRepo.GetCheeps(author: testName);
    
        Assert.All(cheeps, c => Assert.Equal(testName, c.Author.Username));
        Assert.DoesNotContain(cheeps, c => c.Author.Username == "SomeoneElse");
    }

    [Fact]
    public async Task CheepsOver160CharsAreRejected()
    {
        var tooLongCheep =
            "sdjlaksjdlkajS;lkajslnvfnvkjnfsbfbjkfghfdjghgoijwoeijijlkjdflkjadslkfjnjvnfdjhboigjwegerekdfkf;ldkf;ksd;fkds;kf;sdkfpoekfpwejgpejgraopbnofnbdajvfvjofnbdfonbldfkj";
        await _authorRepo.CreateUser(testName, testMail);

        await _cheepRepo.PostCheep(tooLongCheep, testName, testMail);
        var cheeps = await _cheepRepo.GetCheeps();
        Assert.Empty(cheeps);
           
        //await Assert.ThrowsAsync<ArgumentException>(() => _cheepRepo.PostCheep(tooLongCheep, testName, testMail));
    }

    [Fact]
    public async Task GetPaginatedCheeps()
    {
        await  _authorRepo.CreateUser(testName, testMail);
        await _cheepRepo.PostCheep("test", testName, testMail);
        var authorCheeps = await _cheepRepo.GetCheeps(testName);
        var pageCheeps = await _cheepRepo.GetPaginatedCheeps(1, 32, testName);
        Assert.Equal(authorCheeps, pageCheeps);
    }

    [Fact]
    public async Task GetCheepsFromAuthorAndFollowing()
    {
        await _authorRepo.CreateUser(testName, testMail);
        await _authorRepo.CreateUser("b", "b@b.com");
        var userA = await _context.Authors.FirstAsync(a => a.UserName == testName);
        await _cheepRepo.PostCheep("test", testName, testMail);
        await _cheepRepo.PostCheep("b", "b", "b@b.com");
        var userB = await _context.Authors.FirstAsync(a => a.UserName == "b");
        await _authorRepo.FollowUser(userA.Id, userB.Id);
        var cheepAs = await _cheepRepo.GetCheeps(testName);
        var cheepBs = await _cheepRepo.GetCheeps("b");
        var pageCheeps = await _cheepRepo.GetCheepsFromAuthorAndFollowing(1, 32, testName);
        var cheepA = cheepAs[0];
        var cheepB = cheepBs[0];
        Assert.Contains(pageCheeps, c =>c.Id == cheepA.Id);
        Assert.Contains(pageCheeps, c =>c.Id == cheepB.Id);
    }
    public void Dispose()
    {
        _context.Database.CloseConnection();
        _context.Dispose();
    }
}