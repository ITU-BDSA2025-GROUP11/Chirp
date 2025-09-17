using System.Globalization;
using CsvHelper;
using DocoptNet;

namespace Chirp.CSVDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly string path;
    private readonly List<T> cheeps;

    public CSVDatabase(string filePath = "../chirp_cli_db.csv")
    {
        // Read before changing file path !! 
        // chirp_cli_db.csv is one remove from root. '../' cds to the parent folder of root and ->
        // and then looks for chirp_cli_db.csv.
        // changing the path to your local path will break the application for everybody else :(
        
        path = filePath;
        cheeps = new List<T>();
    }

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

    /// <summary>
    /// Method to read from a csv-database (it loads previously stored cheeps)
    /// </summary>
    /// <param name="limit">
    /// The limit of items to be read. If left empty, every item is returned
    /// </param>
    /// <returns>
    /// A sequence of objects of type T (fx Cheeps)
    /// </returns>
    public IEnumerable<T> Read(int? limit = null)
    {
        
        try
        {
            StreamReader reader = new(path);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var record = csv.GetRecord<T>(); //Loader recordsene ind???? - hvordan virker dette 🙁
                cheeps.Add(record);
            }
            
        }catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
        
        PrintCheeps(cheeps.Cast<Cheep>().ToList());
        return  cheeps;
    }
    
/// <summary>
/// Method to store a message in a csv-database. 
/// </summary>
/// <param name="record"></param>
    public void Store(T record)
    {
        using var sw = File.AppendText(path);
        using var csv = new CsvWriter(sw, CultureInfo.InvariantCulture);
        
        csv.WriteRecord(record);
        csv.NextRecord();
    }


//Følgende metoder skal refaktorises ifm. opgave 2C:
    public static void PrintCheeps(List<Cheep> cheeps)
    {
        try
        {
            foreach (var cheep in cheeps)
            {
                //Denne kan måske gøres pænere...
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
