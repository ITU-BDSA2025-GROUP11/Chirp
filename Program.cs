
using System.IO;

class Program
{
    private static String path;
    private static String author;
    private static String dateTime;
    private static String message;
    static void Main(String[] args)
    {
        // path needs to be fixed
        path = "/Users/miljajensen/3s/bdsa/Chirp/chirp_cli_db.csv";
        if (args.Length > 1)
        { 
            Cheep(args[1]);
            
        } else ReadFromFile();
    }
    static void Cheep(String cheep)
    {
        Console.WriteLine(cheep);
        WriteToFile(cheep);
    }
    static void WriteToFile(String cheep)
    {
        using (StreamWriter sw = File.AppendText(path))
        {
            cheep = '"' + cheep + '"';
            author = Environment.UserName;
            sw.WriteLine(author + ',' + cheep);
            
        }	
        
    }
    private static void ReadFromFile()
    {
        try
        {
            StreamReader reader = new(path);
            string? line = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                // Read the stream as a string.
                // Write the text to the console.
                PrintFromFile(line);
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }
    static void PrintFromFile(String line)
    {
        try
        {
            String[] textArray = line.Split('"');
            author = textArray[0].Replace(",", "");
            dateTime = textArray[2].Replace(",", "");
            dateTime = Epoch2dateString(dateTime) + " " + Epoch2timeString(dateTime);
            message = textArray[1];

            var finalString = author + " @ " + dateTime + " " + message;

            Console.WriteLine(finalString);
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
        
    }

    static String Epoch2dateString(String dateTime) 
    {
        int epoch = Int32.Parse(dateTime);
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch).ToShortDateString(); 
    }
    static String Epoch2timeString(String dateTime) 
    {
        int epoch = Int32.Parse(dateTime);
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch).ToLongTimeString(); 
    }
    
}
