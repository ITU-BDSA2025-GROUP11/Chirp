using Chirp.CSVDB;
using Chirp.CLI;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// use your existing CSVDatabase for storage
var db = new CSVDatabase<Cheep>();

// GET /cheeps
app.MapGet("/cheeps", () =>
{
    var cheeps = db.Read();
    return Results.Ok(cheeps);
});

// POST /cheep
app.MapPost("/cheep", (Cheep cheep) =>
{
    db.Store(cheep);
    return Results.Ok();
});

app.Run("http://localhost:5000");
