using Chirp.CLI;
using Chirp.CSVDB;
using DocoptNet;

class Program
{
    
    
    private static void Main(String[] args)
    {
        CSVDatabase<Cheep> cheepDB = new CSVDatabase<Cheep>();
        CLI(args, cheepDB);
    }
    private const string Usage = @"Chirp CLI.

            Usage:
                dotnet.exe chirp <message>
                dotnet.exe print
                dotnet.exe (-h | --help)

            options: 
                -h --help     Show this screen.
                chirp <message>  Post a chirp
                print    Prints chirps from file
    ";
    private static void CLI(string[] args,CSVDatabase<Cheep>  cheepDB)
    {
        try
        {
            var arguments = new Docopt().Apply(Usage, args, version: "Chirp CLI 1.0");

            if (arguments["chirp"].IsTrue)
            {
                Console.WriteLine("Chirping to file: \n");
                var repo = new HttpDatabaseRepository("https://bdsagroup11chirpremotedb-dwg6d7dngqgfhtdh.norwayeast-01.azurewebsites.net/");
                Chirp(arguments, repo);
                if (arguments["print"].IsTrue)
                {
                    PrintFromRepo(arguments, repo);
                }
            }
            else if (arguments["print"].IsTrue)
            {
                PrintFromDb(cheepDB);
            }
        }
        catch (DocoptInputErrorException e)
        {
            Console.WriteLine("No CLI args detected\n\nYou have the following CLI options:\n");
            Console.WriteLine("dotnet run -h or --help        Show this screen.");
            Console.WriteLine("dotnet run chirp <message>      Post a chirp");
            Console.WriteLine("dotnet run print     Prints chirps from file\n");
            Console.WriteLine("Have a good day :)\n");
        }
    }

    private static void Chirp(IDictionary<string,ValueObject> arguments, HttpDatabaseRepository repo)
    {
        Console.WriteLine("Chirping to service:\n");
        Cheep cheep = new Cheep(Environment.UserName, arguments["<message>"] + "", DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        repo.Store(cheep);
    }

    private static void PrintFromRepo(IDictionary<string, ValueObject> arguments, HttpDatabaseRepository repo)
    {
        Console.WriteLine("Printing cheeps from service\n");
        var cheeps = repo.Read();
        UserInterface.PrintCheeps(cheeps);
    }

    private static void PrintFromDb(CSVDatabase<Cheep>  cheepDB)
    {
        Console.WriteLine("Printing chirps from file\n");
        cheepDB.Read();
    }
}
