using Microsoft.AspNetCore.Mvc.Testing;
using Chirp.CLI;
using Xunit;

namespace Chirp.CSVDB.Test;

/// <summary>
/// Integration tests for the CSV DB
/// </summary>
public class CSVDatabaseIntegrationTests
{
    private readonly HttpDatabaseRepository _repo;
    public CSVDatabaseIntegrationTests()
    {
        _repo= new HttpDatabaseRepository("http://localhost:5000");
    }

    public void Get_Request_Returns_HTTP200()
    {
        
    }

    /*
     * a) When you send an HTTP GET request to the /cheeps endpoint the status code of the HTTP response is 200
     * and the response body contains a list of Cheep objects serialized to JSON.
     * b) When you send an HTTP POST request to the /cheep endpoint with a request body containing a
     * JSON serialized Cheep object, you receive 200 as status code of the HTTP response.
     */
    
}