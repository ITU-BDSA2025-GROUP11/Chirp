using database;

// if facade initiated with null, a tempDB is created
DBFacade facade = new DBFacade(null);

facade.initDB();
facade.Post("MACHO MAN RANDY SAVAGE");

// if get argument is null, all tweeets are returned
// if argument is author, only those tweets are returned 
var cheeps = facade.Get(null);

// below is just a test print 
foreach (var cheep in cheeps)
{
    Console.WriteLine(cheep);
}