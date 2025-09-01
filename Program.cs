
using System.IO;

class Program
{
    
    static void Main()
    {
    
    }

    // this method is too long needs to be refactored
    static void printFromFile()
    {
            
        string author;
        string dateTime;
        string message;
        try
        {
            // Open the text file using a stream reader.
        
            StreamReader reader = new("/Users/miljajensen/3s/bdsa/Chirp/chirp_cli_db.csv");
            string line = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                // Read the stream as a string.
                string[] textArray = line.Split('"');
                author = textArray[0].Replace(",", "");
                dateTime = textArray[2].Replace(",", "");
                dateTime = epoch2dateString(dateTime) + " " + epoch2timeString(dateTime);
                message = textArray[1];
        
                string finalString = author + " @ " + dateTime + " " + message;
        
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

    static string epoch2dateString(string dateTime) 
    {
        int epoch = Int32.Parse(dateTime);
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch).ToShortDateString(); 
    }
    static string epoch2timeString(string dateTime) 
    {
        int epoch = Int32.Parse(dateTime);
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch).ToLongTimeString(); 
    }

    static void cheep(String[] args)
    {
        
    }
    
}








