using Chirp.CSVDB;

namespace Chirp.CLI;


public class Program
{
    private static void Main(String[] args)
    {
        CSVDatabase<Cheep> cheepDb = new CSVDatabase<Cheep>();
        UserInterface.CLI(args, cheepDb);
    }
    
}
