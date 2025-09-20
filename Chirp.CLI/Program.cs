using Chirp.CSVDB;
namespace Chirp.CLI;

public class Program
{
    private static void Main(String[] args)
    {
        CsvDatabaseIntegration<Cheep> cheepDB = new CsvDatabaseIntegration<Cheep>();
        cheepDB.Cli(args, cheepDB);
    }
}