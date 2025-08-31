/*if (args[0] == "say")
{
    var message = args[1];
    var frequency = int.Parse(args[2]);
    foreach (var i in Enumerable.Range(1, frequency))
        Console.Write(message + " ");
}*/

using System.Text;

class Program
{
    public static void Main(string[] args)
    {
        List<string> log = new List<string>();
        try
        {
            using (StreamReader sr =
                   new StreamReader(
                       "/Users/emilie/Documents/ITU/3. semester/Software Architecture/Chirp/chirp_cli_db.csv"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("Author"))
                    {
                        continue;
                    }
                    string[] chirpInfo = line.Split('"');
                    
                    string username = chirpInfo[0].Trim(',');
                    string message = chirpInfo[1].Trim('"');
                    string timeStamp = Epoch2Datestring(Int32.Parse(chirpInfo[2].Trim(','))).ToString().Substring(0,19);
                
                    log.Add(Format(username, message, timeStamp));
                }
            }
        }
        catch (Exception e)
        {
           // Console.WriteLine("The file could not be read:");
           // Console.WriteLine(e.Message);
        }
        foreach (string s in log)
        {
            Console.WriteLine(s);
        }
        
        
        Console.WriteLine("Do you want to add a new cheep? Type 'cheep + [message]'");
        
        string prompt = Console.ReadLine();
        string[] newMessage;

        if (prompt != null)
        {
            newMessage = prompt.Split(' ');
            
            if (newMessage[0] == "cheep")
            {
                var message = newMessage[1];
               // Console.WriteLine(message);
               
               string path = "/Users/emilie/Documents/ITU/3. semester/Software Architecture/Chirp/chirp_cli_db.csv";
               string username = Environment.UserName;
               string timeStamp = DateTime.Now.ToString();
               Console.WriteLine(Format(username, message, timeStamp));
               File.AppendAllText(path, Format(username, message, timeStamp) + Environment.NewLine);
            }
        }
        
        /*Console.WriteLine("Want to add a new cheep? Y/N");
        string confirmation =  Console.ReadLine();
        if (confirmation.ToLower() == "y")
        {
            newCheep();
        }else if (confirmation.ToLower() == "n")
        {
            Console.WriteLine("End of program");
        }
        else
        {
            Console.WriteLine("Invalid input");
        }*/
    }

    /*private static void newCheep()
    {
        string path = "/Users/emilie/Documents/ITU/3. semester/Software Architecture/Chirp/chirp_cli_db.csv";
        string message = Console.ReadLine();
        string username = Environment.UserName;
        string timeStamp = DateTime.Now.ToString();
        //using (StreamWriter sw = File.AppendAllText(path))
        
        Console.WriteLine(Format(username, message, timeStamp));
        File.AppendAllText(path, Format(username, message, timeStamp));
        	
        // StreamWriter sw = new StreamWriter("/Users/emilie/Documents/ITU/3. semester/Software Architecture/Chirp/chirp_cli_db.csv");
        // String line = Console.ReadLine();

    }*/

    private static DateTimeOffset Epoch2Datestring(int epoch)
    {
        //return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch).ToShortDateString();
       return DateTimeOffset.FromUnixTimeSeconds(epoch).UtcDateTime;
    }

    private static string Format(string username, string message ,string timeStamp)
    {
        return username + " @ " + timeStamp + ": " + message;
    }
}