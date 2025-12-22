using Chirp.Infrastructure;
using Chirp.Web.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;
using Microsoft.Extensions.Logging.Abstractions;

namespace WebTest;

public class UserTimelineTest
{
    ICheepRepository? _cheepRepo;
    IAuthorRepository? _authorRepo;
    public required UserTimelineModel UserTimeline;
    ChirpDbContext? _context;
    
    public required CheepService CheepService;
    public required AuthorService AuthorService;

    private void Before()
    {
        var options = new DbContextOptionsBuilder<ChirpDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        _context = new ChirpDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated(); 

        _cheepRepo = new CheepRepository(_context, new LoggerFactory());
        _authorRepo = new AuthorRepository(_context, new LoggerFactory());
        AuthorService = new AuthorService(_authorRepo, NullLogger<AuthorService>.Instance);
        CheepService = new CheepService(_cheepRepo, NullLogger<CheepService>.Instance);
    }
    [Fact]
    public void TestUserTimelineInstantiation()
    {
        UserTimeline = new UserTimelineModel(CheepService, AuthorService) {
            Message = "What should this say"
        };
        
        Assert.NotNull(UserTimeline);
    }
    [Fact]
    public async Task TotalNumberOfCheepsTest()
    {
        Before();
        UserTimeline = new UserTimelineModel(CheepService, AuthorService) {
            Message = "What should this say"
        };
        var cheeps = await _cheepRepo!.GetCheeps();
        var numberOfCheeps = cheeps.Count;
        Assert.Equal(numberOfCheeps, UserTimeline.NumberOfCheeps);
    }
    
    [Theory]
    [InlineData(300, 32)]
    [InlineData(320, 32)]
    [InlineData(5, 10)]
    public void TotalNumberOfPagesLogicTest(int numberOfCheeps, int cheepsPerPage)
    {
        Before();
        UserTimeline = new UserTimelineModel(CheepService, AuthorService) {
            Message = "What should this say"
        };
        var numberOfFullPages =  numberOfCheeps / cheepsPerPage;
        var excessCheeps = numberOfCheeps % cheepsPerPage;
        var expectedNumberOfPages = excessCheeps > 0 ? numberOfFullPages+1 : numberOfFullPages;
        
        Assert.Equal(expectedNumberOfPages, UserTimeline.GetTotalPages(numberOfCheeps, cheepsPerPage));
        
    }
}