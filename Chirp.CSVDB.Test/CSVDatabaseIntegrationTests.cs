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
    private CsvDatabaseIntegration<Cheep> _cheepDB;
   
    public CSVDatabaseIntegrationTests()
    {
        _cheepDB = new CsvDatabaseIntegration<Cheep>();
    }
    
    [Fact]
    public void Read_Get_Request_Returns_HTTP200()
    { 
        string[] args = { "chirp","print" };
        _cheepDB.Cli(args, _cheepDB);
        var responseMsg = _cheepDB.GetRepository().getResponseMsg();

        Assert.True(responseMsg.StatusCode.Equals(200));
    }

    /*
     * a) When you send an HTTP GET request to the /cheeps endpoint the status code of the HTTP response is 200
     * and the response body contains a list of Cheep objects serialized to JSON.
     * b) When you send an HTTP POST request to the /cheep endpoint with a request body containing a
     * JSON serialized Cheep object, you receive 200 as status code of the HTTP response.
     */
    
}