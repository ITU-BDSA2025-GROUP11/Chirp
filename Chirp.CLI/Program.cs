using SimpleDB;


class Program
{
    private static String path;
    private static String author;
    private static String message;
    private static long epochTime;
    private static List<Cheep> cheeps;
    private static CSVDatabase<Cheep> CsvDatabase;


    private static void Main(String[] args)
    {
        CSVDatabase<Cheep> cheepDB = new CSVDatabase<Cheep>();
        cheepDB.Cli(args, cheepDB);
    }
}
