using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Xunit;
using Xunit.Abstractions;


namespace PagesTest;


public class PublicPagesTest
{
    ICheepRepository _cheepRepo;
    IAuthorRepository _authorRepo;
    PublicModel publicPage;
    ChirpDbContext _context;
    private readonly UserManager<Author> _userManager;
    private readonly ITestOutputHelper output;

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
    }
    [Fact]
    public void PublicPageInstantiationTest()
    {
        Before();
        publicPage = new PublicModel(_cheepRepo, _authorRepo, _userManager)
        {
            Message = "What should this say"
        };
        Assert.NotNull(publicPage);
    }
    
    
    [Fact]
    public async void TotalNumberOfCheepsTest()
    {
        try
        {
            Before();
            publicPage = new PublicModel(_cheepRepo, _authorRepo, _userManager)
            {
                Message = "What should this say"
            };
            var cheeps = await _cheepRepo.GetCheeps();
            var numberOfCheeps = cheeps.Count;
            Assert.Equal(numberOfCheeps, publicPage.NumberOfCheeps);
        }
        catch (Exception ex)
        {
            //ITestOutputHelper outputHelper = new TestOutputHelper();
           // outputHelper.WriteLine(ex.Message);
           output.WriteLine(ex.Message);
        }
        
    }

    [Theory]
    [InlineData(300, 32)]
    [InlineData(320, 32)]
    [InlineData(5, 10)]
    public void TotalNumberOfPagesLogicTest(int numberOfCheeps, int cheepsPerPage)
    {
        Before();
        publicPage = new PublicModel(_cheepRepo, _authorRepo, _userManager)
        {
            Message = "What should this say"
        };
        var numberOfFullPages =  numberOfCheeps / cheepsPerPage;
        var excessCheeps = numberOfCheeps % cheepsPerPage;
        var expectedNumberOfPages = excessCheeps > 0 ? numberOfFullPages+1 : numberOfFullPages;
        
        Assert.Equal(expectedNumberOfPages, publicPage.GetTotalPages(numberOfCheeps, cheepsPerPage));
        
    }
}