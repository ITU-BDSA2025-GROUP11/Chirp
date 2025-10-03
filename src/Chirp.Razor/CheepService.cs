using System;
using System.Collections.Generic;
using System.Linq;
using database;
using Models;


//public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetPaginatedCheeps(int currentPage, int pageSize);
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    static DBFacade facade = new DBFacade(null);
    private static readonly List<CheepViewModel> _cheeps = facade.Get();
    
    public CheepService()
    {
        facade.initDB();
        
    }

    public List<CheepViewModel> GetCheeps()
    {
        return _cheeps;
    }

    public List<CheepViewModel> GetPaginatedCheeps(int currentPage, int pageSize = 32)
    {
        return facade.ExecuteQuery("SELECT * FROM message" +
                            " INNER JOIN user ON message.author_id=user.user_id" +
                            $" ORDER BY message.pub_date DESC LIMIT 32 OFFSET 32 *" + currentPage);
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
