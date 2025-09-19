using Microsoft.AspNetCore.Mvc.Testing;
using Chirp.CLI;

namespace Chirp.CSVDB.Test;

/// <summary>
/// Integration tests for the CSV DB
/// </summary>
public class CSVDatabaseTest
{
    /*
     * a) When you send an HTTP GET request to the /cheeps endpoint the status code of the HTTP response is 200
     * and the response body contains a list of Cheep objects serialized to JSON.
     * b) When you send an HTTP POST request to the /cheep endpoint with a request body containing a JSON serialized
     * Cheep object, you receive 200 as status code of the HTTP response.
     */
   
}