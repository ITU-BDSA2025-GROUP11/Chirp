using Chirp.CSVDB;

public class Program
{
    private static void Main(String[] args)
    {
        CSVDatabase<Cheep> cheepDB = new CSVDatabase<Cheep>();
        cheepDB.Cli(args, cheepDB);
    }
}

