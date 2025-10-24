using Xunit;
using Chirp.Razor.Pages;
namespace PagesTest;


public class PublicPagesTest
{
    ICheepService  service = new CheepService();
    PublicModel publicPage;
    [Fact]
    public void TotalNumberOfCheepsTest()
    {
        publicPage = new PublicModel(service);
        int numberOfCheeps = service.GetCheeps().Count;
        Assert.Equal(numberOfCheeps, publicPage.NumberOfCheeps);
    }

    [Theory]
    [InlineData(300, 32)]
    [InlineData(320, 32)]
    [InlineData(5, 10)]
    public void TotalNumberOfPagesLogicTest(int numberOfCheeps, int cheepsPerPage)
    {
        publicPage = new PublicModel(service);
        int numberOfFullPages =  numberOfCheeps / cheepsPerPage;
        int excessCheeps = numberOfCheeps % cheepsPerPage;
        int expectedNumberOfPages = excessCheeps > 0 ? numberOfFullPages+1 : numberOfFullPages;
        
        Assert.Equal(expectedNumberOfPages, publicPage.GetTotalPages(numberOfCheeps, cheepsPerPage));
        
    }

    // [Theory]
    // [InlineData(32, 4)]
    // [InlineData(10, 10)]
    // public void CurrentPageCheepsTest(int cheepsPerPage, int currentPage)
    // {
    //      this test is not quite thought through
    //     publicPage = new PublicModel(service);
    //     publicPage.PageSize = cheepsPerPage;
    //     publicPage.OnGet(currentPage);
    //     Assert.Equal(cheepsPerPage, publicPage.CurrentPageCheeps.Count);
    // }
}