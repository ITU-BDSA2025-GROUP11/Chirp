using System;
using System.IO;

namespace Chirp.CLI;
using System;
using System.IO;

class Program
{
    public static void Main()
    {
        const string path = "C:\\Users\\joakim\\Desktop\\Chirp\\chirp_cli_db.csv";
        StreamReader sr = new(path);
        
        string? line =  sr.ReadLine();

        while (line != null)
        {
            Console.WriteLine(line);
            
            line = sr.ReadLine();
        }
    }
}

    

    
