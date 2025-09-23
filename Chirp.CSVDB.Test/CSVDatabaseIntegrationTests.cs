using System.Net;
using Newtonsoft.Json;
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
    public void Read_Get_Request_Returns_Success()
    { 
        var repo = new HttpDatabaseRepository("http://localhost:5000");

        var cheeps = repo.Read();

        Assert.Equal(HttpStatusCode.OK, repo.getLastStatusCode());
    }

    [Fact]
    public void Read_Returns_Cheeps_Serialized_To_JSON()
    {
        var repo = new HttpDatabaseRepository("http://localhost:5000");

        var cheeps = repo.Read();
        var jsonFormat = false;

        foreach (var cheep in cheeps)
        {
            try
            {
                DeserializeObject(cheep);
                jsonFormat = true;;
            }

            //https://www.newtonsoft.com/json/help/html/serializingjson.htm
            /* if ((cheep.ToString().StartsWith("{") && cheep.ToString().EndsWith("}"))){
                 if (parts[0].)
                 {

                 }
             }*/
        }
        
    }

    [Fact]
    public void Store_Post_CheepAsJSON_Request_Returns_Success()
    {
        var repo = new HttpDatabaseRepository("http://localhost:5000");
        
        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        int secondsSinceEpoch = (int)t.TotalSeconds;
        
        Cheep cheep = new Cheep("Author", "Test", secondsSinceEpoch);
        repo.Store(cheep); //cheep is serialized as JSON
        Assert.Equal(HttpStatusCode.OK, repo.getLastStatusCode());
    }
    
    /*
     * a) When you send an HTTP GET request to the /cheeps endpoint the status code of the HTTP response is 200
     * and the response body contains a list of Cheep objects serialized to JSON.
     * b) When you send an HTTP POST request to the /cheep endpoint with a request body containing a
     * JSON serialized Cheep object, you receive 200 as status code of the HTTP response.
     */
}