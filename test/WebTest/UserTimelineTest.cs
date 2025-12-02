using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;
using Microsoft.Extensions.Logging.Abstractions;

namespace PagesTest;

public class UserTimelineTest
{
    ICheepRepository _cheepRepo;
    IAuthorRepository _authorRepo;
    UserTimelineModel userTimeline;
    ChirpDbContext _context;
    
    private readonly UserManager<Author> _userManager;
    public required CheepService _cheepService;
    public required AuthorService _authorService;

    public void Before()
    {
        var options = new DbContextOptionsBuilder<ChirpDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        _context = new ChirpDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated(); 

        _cheepRepo = new CheepRepository(_context, new LoggerFactory());
        _authorRepo = new AuthorRepository(_context, new LoggerFactory());
        _authorService = new AuthorService(_authorRepo, NullLogger<AuthorService>.Instance);
        _cheepService = new CheepService(_cheepRepo, NullLogger<CheepService>.Instance);
    }
    [Fact]
    public void TestUserTimelineInstantiation()
    {
        userTimeline = new UserTimelineModel(_cheepService, _authorService, _userManager) {
            Message = "What should this say"
        };
        
        Assert.NotNull(userTimeline);
    }
    [Fact]
    public async Task TotalNumberOfCheepsTest()
    {
        Before();
        userTimeline = new UserTimelineModel(_cheepService, _authorService, _userManager) {
            Message = "What should this say"
        };
        var cheeps = await _cheepRepo.GetCheeps();
        var numberOfCheeps = cheeps.Count;
        Assert.Equal(numberOfCheeps, userTimeline.NumberOfCheeps);
    }
    
    [Theory]
    [InlineData(300, 32)]
    [InlineData(320, 32)]
    [InlineData(5, 10)]
    public void TotalNumberOfPagesLogicTest(int numberOfCheeps, int cheepsPerPage)
    {
        Before();
        userTimeline = new UserTimelineModel(_cheepService, _authorService, _userManager) {
            Message = "What should this say"
        };
        var numberOfFullPages =  numberOfCheeps / cheepsPerPage;
        var excessCheeps = numberOfCheeps % cheepsPerPage;
        var expectedNumberOfPages = excessCheeps > 0 ? numberOfFullPages+1 : numberOfFullPages;
        
        Assert.Equal(expectedNumberOfPages, userTimeline.GetTotalPages(numberOfCheeps, cheepsPerPage));
        
    }
}