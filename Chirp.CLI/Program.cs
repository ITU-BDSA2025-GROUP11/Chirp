using Chirp.CLI;
using Chirp.CSVDB;
using DocoptNet;
namespace Chirp.CLI;


public class Program
{
    private static void Main(String[] args)
    {
        CSVDatabase<Cheep> cheepDB = new CSVDatabase<Cheep>();
        UserInterface.CLI(args, cheepDB);
    }
    
}
