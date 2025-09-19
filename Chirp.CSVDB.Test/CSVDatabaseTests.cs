using Microsoft.AspNetCore.Mvc.Testing;
using Chirp.CLI;

namespace Chirp.CSVDB.Test;

/// <summary>
/// Integration tests for the CSV DB
/// </summary>
public class CSVDatabaseTests : IClassFixture<WebApplicationFactory<Program>>
{
    
   /* private readonly WebApplicationFactory<Program> _factory;

    public CSVDatabaseTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TestMethod1()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/values");
    }
*/
    private readonly WebApplicationFactory<Program> _factory;
    public CSVDatabaseTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Index")]
    [InlineData("/About")]
    [InlineData("/Privacy")]
    [InlineData("/Contact")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/html; charset=utf-8", 
            response.Content.Headers.ContentType.ToString());
    }
  /*  private CSVDatabase<Cheep> cheepDB;
    
    public CSVDatabaseTest() {
        cheepDB = new CSVDatabase<Cheep>();
    }

    public void Dispose()
    {
        cheepDB = null;
    }
    
    //For example, add a test case that checks that an entry can be received from the database after it was stored in there.
    [Fact]
    public void Read_
    */
}