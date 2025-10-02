using System;
using System.Collections.Generic;
using System.Linq;
using database;
using Models;


//public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    static DBFacade facade = new DBFacade(null);
    private static List<CheepViewModel> _cheeps;
    
    public CheepService()
    {
        facade.initDB();
        
    }

    // These would normally be loaded from a database for example
    // private static readonly List<CheepViewModel> _cheeps = new()
    //     {
    //        // new CheepViewModel("Helge", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
    //        // new CheepViewModel("Adrian", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
    //        //return facade.Get();
    //        
    //     };

    // private static List<CheepViewModel> StringToCheepList(list<String> list)
    // {
    //     cheeps = new List<CheepViewModel>();
    //     foreach (string cheepString in list)
    //     {
    //         cheepString.Split(" ");
    //         var user;
    //         var message;
    //         var timestamp;
    //     }
    // }


    public List<CheepViewModel> GetCheeps()
    {
       //return facade.Get();
       return facade.Get32LatestCheeps();
       // return _cheeps;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return _cheeps.Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
