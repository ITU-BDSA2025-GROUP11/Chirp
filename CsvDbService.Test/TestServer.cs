using Chirp.CSVDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

CSVDatabase<Cheep> db = new CSVDatabase<Cheep>();

string testFilePath = Path.Combine(Directory.GetCurrentDirectory(), "test_data.csv");

db.setpath(testFilePath);
db.setUrl("http://localhost:5001");

File.WriteAllText(testFilePath, "Author,Message,Timestamp\n");

app.MapGet("/cheeps", () =>
{
    var cheeps = db.Read();
    return Results.Ok(cheeps);
});

app.MapPost("/cheep", (Cheep cheep) =>
{
    db.Store(cheep);
    return Results.Ok();
});

app.Run("http://localhost:5001");