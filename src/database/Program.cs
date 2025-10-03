using database;

// if facade initiated with null, a tempDB is created
string? dbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH");

DBFacade facade = new DBFacade();

facade.Post("MACHO MAN RANDY SAVAGE");

// if get argument is null, all tweeets are returned
// if argument is author, only those tweets are returned 
var cheeps = facade.Get("Jacqualine Gilcoine");

// below is just a test print 
foreach (var cheep in cheeps)
{
    Console.WriteLine(cheep);
}