
using System.IO;

class Program
{
    
    static void Main(String[] args)
    {
        if (args[0] == "cheep")
        { 
            Cheep(args[1]);
            
        } else PrintFromFile();
    }

    // this method is too long needs to be refactored
    static void PrintFromFile()
    {
            
        string author;
        string dateTime;
        string message;
        try
        {
            // Open the text file using a stream reader.
        
            StreamReader reader = new("/Users/miljajensen/3s/bdsa/Chirp/chirp_cli_db.csv");
            var line = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                // Read the stream as a string.
                String[] textArray = line.Split('"');
                author = textArray[0].Replace(",", "");
                dateTime = textArray[2].Replace(",", "");
                dateTime = Epoch2dateString(dateTime) + " " + Epoch2timeString(dateTime);
                message = textArray[1];
        
                var finalString = author + " @ " + dateTime + " " + message;
        
                // Write the text to the console.
                Console.WriteLine(finalString);
                
            }
           
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }

    static string Epoch2dateString(string dateTime) 
    {
        int epoch = Int32.Parse(dateTime);
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch).ToShortDateString(); 
    }
    static string Epoch2timeString(string dateTime) 
    {
        int epoch = Int32.Parse(dateTime);
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch).ToLongTimeString(); 
    }

    static void Cheep(string cheep)
    {
        Console.WriteLine(cheep);
    }
    
}








