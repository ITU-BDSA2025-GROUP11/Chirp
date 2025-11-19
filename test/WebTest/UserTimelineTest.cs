using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PagesTest;

public class UserTimelineTest
{
    ICheepRepository _repo;
    UserTimelineModel userTimeline;
    ChirpDbContext _context;
    private readonly UserManager<Author> _userManager;

    public void Before()
    {
        var options = new DbContextOptionsBuilder<ChirpDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        _context = new ChirpDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated(); 

        _repo = new CheepRepository(_context, new LoggerFactory());
    }
    [Fact]
    public void TestUserTimelineInstantiation()
    {
        userTimeline = new UserTimelineModel(_repo, _userManager);
        Assert.NotNull(userTimeline);
    }
    [Fact]
    public void TotalNumberOfCheepsTest()
    {
        Before();
        userTimeline = new UserTimelineModel(_repo, _userManager);
        var numberOfCheeps = _repo.GetCheeps().Count;
        Assert.Equal(numberOfCheeps, userTimeline.NumberOfCheeps);
    }
    [Theory]
    [InlineData(300, 32)]
    [InlineData(320, 32)]
    [InlineData(5, 10)]
    public void TotalNumberOfPagesLogicTest(int numberOfCheeps, int cheepsPerPage)
    {
        Before();
        userTimeline = new UserTimelineModel(_repo,  _userManager);
        var numberOfFullPages =  numberOfCheeps / cheepsPerPage;
        var excessCheeps = numberOfCheeps % cheepsPerPage;
        var expectedNumberOfPages = excessCheeps > 0 ? numberOfFullPages+1 : numberOfFullPages;
        
        Assert.Equal(expectedNumberOfPages, userTimeline.GetTotalPages(numberOfCheeps, cheepsPerPage));
        
    }
}