/*List<string> cheeps = new() { "Hello, ADBSA students!", "Welcome to the course!", "I hope you had a good summer." };
foreach (var cheep in cheeps)
{
    Console.WriteLine(cheep);
    Thread.Sleep(1000);
    // PLEASE PLEASE PLEASE
    
    
}*/
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

class Program
{
    public static void Main()
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
                    string timeStamp = epoch2Datestring(Int32.Parse(chirpInfo[2].Trim(','))).ToString().Substring(0,19);
                    //Console.WriteLine(date);
                    log.Add(username + " @ " + timeStamp + ": " + message);
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
    }
    
    private static DateTimeOffset epoch2Datestring(int epoch)
    {
        //return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch).ToShortDateString();
       return DateTimeOffset.FromUnixTimeSeconds(epoch).UtcDateTime;
       
    }
}