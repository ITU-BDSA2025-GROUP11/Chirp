using Chirp.CSVDB;
namespace Chirp.CLI;

public class Program
{
    private static void Main(String[] args)
    {
        CSVDatabase<Cheep> cheepDB = new CSVDatabase<Cheep>();
        cheepDB.Cli(args, cheepDB);
    }
}