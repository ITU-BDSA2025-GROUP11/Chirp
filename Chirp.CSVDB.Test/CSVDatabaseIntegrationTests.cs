using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Chirp.CLI;
using Xunit;
using System.Net.Http;

namespace Chirp.CSVDB.Test;

/// <summary>
/// Integration tests for the CSV DB
/// </summary>
public class CSVDatabaseIntegrationTests
{
    private CSVDatabase<Cheep> _cheepDB;
    private HttpDatabaseRepository repo;
    
   
    public CSVDatabaseIntegrationTests()
    {
        _cheepDB = new CSVDatabase<Cheep>();
        repo = new HttpDatabaseRepository("http://localhost:5000");
    }
    
    [Fact]
    public void Read_Get_Request_Returns_HTTP200()
    { 
        var repo = new HttpDatabaseRepository("http://localhost:5000");

        var cheeps = repo.Read();

        Assert.Equal(HttpStatusCode.OK, repo.getLastStatusCode());
    }
    
    /*
     * a) When you send an HTTP GET request to the /cheeps endpoint the status code of the HTTP response is 200
     * and the response body contains a list of Cheep objects serialized to JSON.
     * b) When you send an HTTP POST request to the /cheep endpoint with a request body containing a
     * JSON serialized Cheep object, you receive 200 as status code of the HTTP response.
     */
}