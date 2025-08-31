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

            var message = "";
            if (newMessage[0] == "cheep")
            {
                for (int i = 1; i < newMessage.Length; i++)
                {
                    message = message + newMessage[i]+ ' ';
                } 
                
                string path = "/Users/emilie/Documents/ITU/3. semester/Software Architecture/Chirp/chirp_cli_db.csv";
                string username = Environment.UserName;
                string timeStamp = DateTime.Now.ToString();
                Console.WriteLine(Format(username, message, timeStamp));
                File.AppendAllText(path, Format(username, message, timeStamp) + Environment.NewLine);
            }
        }
        
    }
    
    private static DateTimeOffset Epoch2Datestring(int epoch)
    {
        return DateTimeOffset.FromUnixTimeSeconds(epoch).UtcDateTime;
    }

    private static string Format(string username, string message ,string timeStamp)
    {
        return username + " @ " + timeStamp + ": " + message;
    }
}