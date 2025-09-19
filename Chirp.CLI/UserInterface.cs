using Chirp.CSVDB;

namespace Chirp.CLI;

public static class UserInterface
{
        public void Cli(string[] args,CSVDatabase<Cheep>  cheepDB)
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
            // CLI
            var arguments = new Docopt().Apply(usage, args, version: "Chirp CLI 1.0");

            if (arguments["chirp"].IsTrue)
            {
                Console.WriteLine("Chirping to file: \n");
                var repo = new HttpDatabaseRepository("http://localhost:5000"); // later Azure URL

                if (arguments["chirp"].IsTrue)
                {
                    Console.WriteLine("Chirping to service:\n");
                    Cheep cheep = new Cheep(Environment.UserName, arguments["<message>"] + "", DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    repo.Store(cheep);
                }
                else if (arguments["print"].IsTrue)
                {
                    Console.WriteLine("Printing chirps from service\n");
                    var cheeps = repo.Read();
                    CSVDatabase<Cheep>.PrintCheeps(cheeps.ToList());
                }
            }
            else if (arguments["print"].IsTrue)
            {
                Console.WriteLine("Printing chirps from file\n");
                Read();
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
    public static void PrintCheeps(List<Cheep> cheeps)
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
    
    
}