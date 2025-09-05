using System.Globalization;
using CsvHelper;

namespace SimpleDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    private static String path;
    private static List<Cheep> cheeps;
    
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
       
    }
/// <summary>
/// Method to store a message in a csv-database. 
/// </summary>
/// <param name="record"></param>
    public void Store(T record)
    {
        path= "chirp_cli_db.csv";
        using var sw = File.AppendText(path);
        using var csv = new CsvWriter(sw, CultureInfo.InvariantCulture);
        
        csv.WriteRecord(new Cheep(Environment.UserName, input, DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
        csv.NextRecord();
        
    }
}