using System.Globalization;
using CsvHelper;
using DocoptNet;

namespace Chirp.CSVDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    private static String path;
    private static List<T> cheeps;

    public CSVDatabase()
    {
        // Read before changing file path !! 
        // chirp_cli_db.csv is one remove from root. '../' cds to the parent folder of root and ->
        // and then looks for chirp_cli_db.csv.
        // changing the path to your local path will break the application for everybody else :(
        
        path = "../chirp_cli_db.csv";
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
                var record = csv.GetRecord<T>();
                cheeps.Add(record);
            }
            
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
}
