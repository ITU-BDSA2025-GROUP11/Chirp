using System.Collections;
using System.Globalization;
using System.IO;
using CsvHelper;
using SimpleDB;

class Program
{
    private static String path;
    private static String author;
    private static String message;
    private static long epochTime;
    private static List<Cheep> cheeps;
    private static CSVDatabase<Cheep> CsvDatabase;

    private static void Main(String[] args)
    {
        CsvDatabase = new CSVDatabase<Cheep>();
        Console.WriteLine("Welcome to Chirp!");
        if (args.Length > 0) //Hvis man selv skriver en besked i terminalen
        {
            var userInput = args[0];
            Cheep cheep = new Cheep(Environment.UserName, userInput, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            CsvDatabase.Store(cheep);
        }
        else
        {
            CsvDatabase.Read();
        }
    }
}
