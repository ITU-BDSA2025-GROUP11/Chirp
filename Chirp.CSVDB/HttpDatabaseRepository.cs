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
        var cheeps = _client.GetFromJsonAsync<List<Cheep>>("/cheeps").Result ?? new List<Cheep>();
        // HttpResponseMessage response = _client.GetFromJsonAsync("/cheeps").Result;   

        return limit.HasValue ? cheeps.Take(limit.Value) : cheeps;
    }

    public void Store(Cheep record)
    {
        var response = _client.PostAsJsonAsync("/cheep",record).Result;
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to post cheep. Status: {response.StatusCode}");
        }
    }
}