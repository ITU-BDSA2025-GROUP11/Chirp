using System.Globalization;
using CsvHelper;

namespace SimpleDB;

sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    public IEnumerable<T> Read(int? limit = null)
    {
       /* using var sw = File.AppendText(path);
        using var csv = new CsvWriter(sw, CultureInfo.InvariantCulture);
        csv.WriteRecord(new Cheep(Environment.UserName,input, DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
        csv.NextRecord();*/
       return null;
    }

    public void Store(T record)
    {
    }
}