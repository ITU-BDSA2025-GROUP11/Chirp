using System;
using System.Collections.Generic;
using System.Linq;
using database;
using Models;

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    
    DBFacade facade = new DBFacade();

    public CheepService()
    {
        facade.initDB();
    }
    // These would normally be loaded from a database for example
    //private static readonly List<CheepViewModel> _cheeps = facade.Get();

    public List<CheepViewModel> GetCheeps()
    {
        return facade.Get();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return facade.Get(author);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
