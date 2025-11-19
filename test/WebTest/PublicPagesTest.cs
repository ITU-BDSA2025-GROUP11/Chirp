using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PagesTest;


public class PublicPagesTest
{
    ICheepRepository _repo;
    PublicModel publicPage;
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
    public void PublicPageInstantiationTest()
    {
        publicPage = new PublicModel(_repo, _userManager);
        Assert.NotNull(publicPage);
    }
    
    [Fact]
    public void TotalNumberOfCheepsTest()
    {
        Before();
        publicPage = new PublicModel(_repo, _userManager);
        var numberOfCheeps = _repo.GetCheeps().Count;
        Assert.Equal(numberOfCheeps, publicPage.NumberOfCheeps);
    }

    [Theory]
    [InlineData(300, 32)]
    [InlineData(320, 32)]
    [InlineData(5, 10)]
    public void TotalNumberOfPagesLogicTest(int numberOfCheeps, int cheepsPerPage)
    {
        Before();
        publicPage = new PublicModel(_repo,  _userManager);
        var numberOfFullPages =  numberOfCheeps / cheepsPerPage;
        var excessCheeps = numberOfCheeps % cheepsPerPage;
        var expectedNumberOfPages = excessCheeps > 0 ? numberOfFullPages+1 : numberOfFullPages;
        
        Assert.Equal(expectedNumberOfPages, publicPage.GetTotalPages(numberOfCheeps, cheepsPerPage));
        
    }
}