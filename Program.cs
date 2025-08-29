namespace Chirp.CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
           
            string path = "C:/Users/there/Documents/GitHub/Chirp/chirp_cli_db.csv";
            StreamReader reader = null;

            if (File.Exists(path))
            {
                reader = new StreamReader(File.OpenRead(path));
                
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] word = line.Replace(",", "").Split('"');
                    foreach (var v in word) { // Formater ordenligt
                        Console.Write(v + " ");
                    }
                    Console.WriteLine();
                }

            }
            else
            {
                Console.WriteLine("gg bror");
            }
        }
    }
}

    

