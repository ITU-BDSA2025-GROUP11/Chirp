using System.Net;
using System.Net.Http.Json;

namespace Chirp.CSVDB;

public class HttpDatabaseRepository : IDatabaseRepository<Cheep>
{
    private readonly HttpClient _client;
    private HttpStatusCode status;

    public HttpDatabaseRepository(string baseUrl)
    {
        _client = new HttpClient { BaseAddress = new Uri(baseUrl)};
    }

    public IEnumerable<Cheep> Read(int? limit = null)
    {
        var response = _client.GetAsync("/cheeps").Result;
        status = response.StatusCode;
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to fetch cheeps. Status: {status}");
        } 
        var cheeps = _client.GetFromJsonAsync<List<Cheep>>("/cheeps").Result ?? new List<Cheep>(); //deserialize 
        return limit.HasValue ? cheeps.Take(limit.Value) : cheeps;
    }

    public void Store(Cheep record)
    {
        var response = _client.PostAsJsonAsync("/cheep",record).Result;
        status = response.StatusCode;
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to post cheep. Status: {response.StatusCode}");
        }
    }
    
    public HttpStatusCode LastStatusCode => status;
}