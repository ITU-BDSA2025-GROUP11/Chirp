using Chirp.CLI;
using Chirp.CSVDB;
using DocoptNet;

class Program
{
    private static void Main(String[] args)
    {
        CSVDatabase<Cheep> cheepDB = new CSVDatabase<Cheep>();
        UserInterface.CLI(args, cheepDB);
    }
    
}
