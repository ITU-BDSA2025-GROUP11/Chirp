using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Chirp.Core.DomainModel;

namespace DatabaseTest;

public class CheepRepositoryIntegrationTests : IDisposable
{
    private readonly ChirpDbContext _context;
    private readonly CheepService _cheepService;
    private readonly AuthorService _authorService;

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
        
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        var cheepRepository = new CheepRepository(_context, loggerFactory);
        var authorRepository = new AuthorRepository(_context, loggerFactory);
        
        _cheepService = new CheepService(cheepRepository, loggerFactory.CreateLogger<CheepService>());
        _authorService = new AuthorService(authorRepository, loggerFactory.CreateLogger<AuthorService>());
    
    }
    //shows that we can add and retrive cheeps from the repo
    [Fact]
    public async Task Add_And_Retrieve_Cheep()
    {
        await _authorService.CreateUser(testName, testMail);
        var author = await _context.Authors.FirstAsync(a => a.UserName == testName);
        await _cheepService.PostCheep("Joakim er faktisk pænt handsome OG har rigtig god humor", author.Id);

        var cheeps = await _cheepService.GetCheeps();

        Assert.Single(cheeps);
        Assert.Equal("Joakim er faktisk pænt handsome OG har rigtig god humor", cheeps[0].Text);
    }

    //Checks that our cheeps get assigned to the corrct author
    [Fact]
    public async Task Cheep_Belongs_To_Correct_Author()
    {
        await _authorService.CreateUser(testName, testMail);
        var author = await _context.Authors.FirstAsync(a => a.UserName == testName);
        await _cheepService.PostCheep("Resten af gruppen er også pænt cool",  author.Id);

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

        await Assert.ThrowsAsync<ArgumentException>(() => _cheepService.PostCheep("Plz no crash test", "test"));
    }

    //checks that we only get cheeps from the desired author 
    [Fact]
    public async Task Get_Cheeps_From_Author()
    {
        await _authorService.CreateUser(testName, testMail);
        var author = await _context.Authors.FirstAsync(a => a.UserName == testName);
        await _cheepService.PostCheep("Joakim’s cheep 1",  author.Id);
        await _cheepService.PostCheep("Joakim’s cheep 2",   author.Id);
    
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
    
        var cheeps = await _cheepService.GetCheeps(author: testName);
    
        Assert.All(cheeps, c => Assert.Equal(testName, c.Author.Username));
        Assert.DoesNotContain(cheeps, c => c.Author.Username == "SomeoneElse");
    }

    [Fact]
    public async Task CheepsOver160CharsAreRejected()
    {
        var tooLongCheep =
            "sdjlaksjdlkajS;lkajslnvfnvkjnfsbfbjkfghfdjghgoijwoeijijlkjdflkjadslkfjnjvnfdjhboigjwegerekdfkf;ldkf;ksd;fkds;kf;sdkfpoekfpwejgpejgraopbnofnbdajvfvjofnbdfonbldfkj";
        await _authorService.CreateUser(testName, testMail);
        var author = await _context.Authors.FirstAsync(a => a.UserName == testName);
        await _cheepService.PostCheep(tooLongCheep, author.Id);
        var cheeps = await _cheepService.GetCheeps();
        Assert.Empty(cheeps);
           
        //await Assert.ThrowsAsync<ArgumentException>(() => _cheepRepo.PostCheep(tooLongCheep, testName, testMail));
    }

    [Fact]
    public async Task GetPaginatedCheeps()
    {
        await  _authorService.CreateUser(testName, testMail);
        var author = await _context.Authors.FirstAsync(a => a.UserName == testName);
        await _cheepService.PostCheep("test", author.Id);
        var authorCheeps = await _cheepService.GetCheeps(testName);
        var pageCheeps = await _cheepService.GetPaginatedCheeps(1, 32, testName);
        Assert.Contains(pageCheeps, c => c.Id == authorCheeps[0].Id);
    }

    // [Fact]
    // public async Task GetCheepsFromAuthorAndFollowing()
    // {
    //     await _authorService.CreateUser(testName, testMail);
    //     await _authorService.CreateUser("b", "b@b.com");
    //     var userA = await _context.Authors.FirstAsync(a => a.UserName == testName);
    //     await _cheepService.PostCheep("test", userA.Id);
    //     var userB = await _context.Authors.FirstAsync(a => a.UserName == "b");
    //     await _cheepService.PostCheep("b", userB.Id);
    //     await _authorService.FollowUser(userA.Id, userB.Id);
    //     var cheepAs = await _cheepService.GetCheeps(testName);
    //     var cheepBs = await _cheepService.GetCheeps("b");
    //     var pageCheeps = await _cheepService.GetCheepsFromAuthorAndFollowing(1, 32, testName);
    //     var cheepA = cheepAs[0];
    //     var cheepB = cheepBs[0];
    //     Assert.Contains(pageCheeps, c =>c.Id == cheepA.Id);
    //     Assert.Contains(pageCheeps, c =>c.Id == cheepB.Id);
    // }
    
    public void Dispose()
    {
        _context.Database.CloseConnection();
        _context.Dispose();
    }
}