namespace Chirp.CLI;
using System;
using System.IO;

class Program
{
    public static void Main()
    {
        const string path = "C:\\Users\\joakim\\Desktop\\Chirp\\chirp_cli_db.csv";
        StreamReader sr = new(path);
        
        sr.ReadLine();
        
        //string? line =  sr.ReadLine();
        string line = "HEJ";
        
        while (line != null)
        {
            line = sr.ReadLine();
            string[] lines = line.Split('"');
            
            string author = lines[0].TrimEnd(',');;
            string message = lines[1].Trim('"'); 
            long timestamp = long.Parse(lines[2].TrimStart(','));
            
            DateTime realTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
            
            Console.WriteLine($"{realTime} @ {author}: {message}");
            
            
        }
    }
}

    

    
