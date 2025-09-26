using Chirp.CSVDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Chirp.CLI;

public class Program
{
    private static void Main(String[] args)
    {
        CSVDatabase<Cheep> cheepDb = new CSVDatabase<Cheep>();
        UserInterface.CLI(args, cheepDb);
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/cheeps", () =>
        {
            IEnumerable<Cheep> cheeps = cheepDb.Read();
            return Results.Json(cheeps);
        });

        app.MapPost("/cheep", (Cheep cheep) =>
        {
            cheepDb.Store(cheep);
        });
            

        app.Run();
    }
    
    
}