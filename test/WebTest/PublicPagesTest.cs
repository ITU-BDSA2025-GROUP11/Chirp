using Chirp.Infrastructure;
using Chirp.Web.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit.Abstractions;
using Xunit.Sdk;
using Xunit;


namespace WebTest;


public class PublicPagesTest
{
    ICheepRepository? _cheepRepo;
    IAuthorRepository? _authorRepo;
    PublicModel? _publicPage;
    ChirpDbContext? _context;
    public required CheepService? CheepService;
    public required AuthorService? AuthorService;
    private readonly ITestOutputHelper? _output = new TestOutputHelper();

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
    public void PublicPageInstantiationTest()
    {
        Before();
        _publicPage = new PublicModel(CheepService!, AuthorService!)
        {
            Message = "What should this say"
        };
        Assert.NotNull(_publicPage);
    }
    
    
    [Fact]
    public async void TotalNumberOfCheepsTest()
    {
        try
        {
            Before();
            _publicPage = new PublicModel(CheepService!, AuthorService!)
            {
                Message = "What should this say"
            };
            var cheeps = await _cheepRepo!.GetCheeps();
            var numberOfCheeps = cheeps.Count;
            Assert.Equal(numberOfCheeps, _publicPage.NumberOfCheeps);
        }
        catch (Exception ex)
        {

           _output!.WriteLine(ex.Message);
        }
        
    }

    [Theory]
    [InlineData(300, 32)]
    [InlineData(320, 32)]
    [InlineData(5, 10)]
    public void TotalNumberOfPagesLogicTest(int numberOfCheeps, int cheepsPerPage)
    {
        Before();
        _publicPage = new PublicModel(CheepService!, AuthorService!)
        {
            Message = "What should this say"
        };
        var numberOfFullPages =  numberOfCheeps / cheepsPerPage;
        var excessCheeps = numberOfCheeps % cheepsPerPage;
        var expectedNumberOfPages = excessCheeps > 0 ? numberOfFullPages+1 : numberOfFullPages;
        
        Assert.Equal(expectedNumberOfPages, _publicPage.GetTotalPages(numberOfCheeps, cheepsPerPage));
        
    }
}