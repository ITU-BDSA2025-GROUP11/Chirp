using Chirp.CSVDB;
using DocoptNet;

namespace Chirp.CLI;

public static class UserInterface
{
   
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        try
        {
            foreach (var cheep in cheeps)
            {
                string formattedTime =
                    (Epoch2dateString(cheep.Timestamp) + " " + Epoch2timeString(cheep.Timestamp) + ":");
                Console.WriteLine($"{cheep.Author} @ {formattedTime} {cheep.Message}");
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }
    public static String Epoch2dateString(long dateTime)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(dateTime).ToString("dd-MM-yyyy");
    }

    public static String Epoch2timeString(long dateTime)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(dateTime).ToString("HH:mm:ss");;
    }
    
    
}