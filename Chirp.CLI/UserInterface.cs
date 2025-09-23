using Chirp.CSVDB;
using DocoptNet;

namespace Chirp.CLI;

public static class UserInterface
{
    private static string URL;
   
    public static void CLI(string[] args,CSVDatabase<Cheep>  cheepDB)
    {
        const string usage = @"Chirp CLI.

                Usage:
                    dotnet.exe chirp <message>
                    dotnet.exe print
                    dotnet.exe (-h | --help)

                options: 
                    -h --help     Show this screen.
                    chirp <message>  Post a chirp
                    print    Prints chirps from file
        ";
        try
        {
            var arguments = new Docopt().Apply(usage, args, version: "Chirp CLI 1.0");

            if (arguments["chirp"].IsTrue)
            {
                Console.WriteLine("Chirping to file: \n");
                var repo = new HttpDatabaseRepository("http://localhost:5000");
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
        PrintCheeps(cheeps);
    }

    private static void PrintFromDb(CSVDatabase<Cheep>  cheepDB)
    {
        Console.WriteLine("Printing chirps from file\n");
        cheepDB.Read();
        PrintCheeps(cheepDB.Read());
    }
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        try
        {
            foreach (var cheep in cheeps)
            {
                string formattedTime =
                    (Epoch2dateString(cheep.Timestamp) + " " + Epoch2timeString(cheep.Timestamp) + ":");
                Console.WriteLine($"{cheep.Author} @ {formattedTime} {cheep.Message}");
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }
    public static String Epoch2dateString(long dateTime)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(dateTime).ToString("dd-MM-yyyy");
    }

    public static String Epoch2timeString(long dateTime)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(dateTime).ToString("HH:mm:ss");;
    }

    public static void SetURL(string wantedURL)
    {
        URL = wantedURL;
        
    }
    
}