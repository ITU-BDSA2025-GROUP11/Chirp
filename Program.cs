using System.Collections;
using System.Globalization;
using System.IO;
using CsvHelper;

class Program
{
    private static String path;
    private static String author;
    private static String message;
    private static long epochTime;
    private static List<Cheep> cheeps;
    private static void Main(String[] args)
    {
        // path needs to be fixed
        path ="chirp_cli_db.csv";

        if (args.Length > 0) //Hvis man selv skriver en besked i terminalen
        { 
            SaveCheep(args[0]);
            
        } else ReadFromFile(); //Ellers læser den fra filen
    }
    private static void SaveCheep(string input) //Behøver denne at eksistere?
    {
        WriteToFile(input);
    }

    private static void WriteToFile(string input)
    {
        using var sw = File.AppendText(path);
        using var csv = new CsvWriter(sw, CultureInfo.InvariantCulture);
        csv.WriteRecord(new Cheep(Environment.UserName,input, DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
        csv.NextRecord();
    }
    private static void ReadFromFile()
    {
        try
        {
            cheeps = new List<Cheep>();
            
            StreamReader reader = new(path);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var record = csv.GetRecord<Cheep>(); //Loader recordsene ind???? - hvordan virker dette 🙁
                cheeps.Add(record);
            } 
            UserInterface.PrintCheeps(cheeps);
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }
    
    public record Cheep(string Author, string Message, long Timestamp);
}

class UserInterface()
{
    public static void PrintCheeps(List<Program.Cheep> cheeps)
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

        static String Epoch2dateString(long dateTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(dateTime).ToShortDateString();
        }

        static String Epoch2timeString(long dateTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(dateTime).ToLongTimeString();
        }
        
    }
}
