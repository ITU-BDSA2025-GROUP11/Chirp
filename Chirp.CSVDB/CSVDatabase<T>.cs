using System.Globalization;
using CsvHelper;

namespace Chirp.CSVDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    private static String path;
    private static List<T> cheeps;

    public CSVDatabase()
    {
        path = "/home/therese/Documents/BDSA/Chirp/chirp_cli_db.csv";
        cheeps = new List<T>();
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
            //UserInterface.PrintCheeps(cheeps);
        }catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
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
        static String Epoch2dateString(long dateTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(dateTime).ToShortDateString();
        }

        static String Epoch2timeString(long dateTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(dateTime).ToLongTimeString();
        }
}
