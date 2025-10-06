using System;
using System.Collections.Generic;
using System.Linq;
using database;
using Models;

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    //public List<CheepViewModel> GetPaginatedCheeps(int currentPage, string? author = null);
    public List<CheepViewModel> GetCheepsFromAuthor(string author);

    public List<CheepViewModel> GetPaginatedCheeps(int currentPage = 1, int pageSize = 32, string? author = null);
    //public List<CheepViewModel> GetPaginatedCheepsFromAuthor(string author, int currentPage = 1);
}

public class CheepService : ICheepService
{
    
    static DBFacade facade = new DBFacade(null);
    private static readonly List<CheepViewModel> _cheeps = facade.Get();

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
    // public List<CheepViewModel> OldGetPaginatedCheeps(int currentPage = 1, int pageSize = 32)
    // {
    //     return facade.ExecuteQuery("SELECT * FROM message" +
    //                                " INNER JOIN user ON message.author_id=user.user_id" +
    //                                $" ORDER BY message.pub_date DESC LIMIT 32 OFFSET 32 *" + currentPage);
    // }

    public List<CheepViewModel> GetPaginatedCheeps(int currentPage = 1, int  pageSize = 32, string? author = null)
    {
        return facade.GetLatestInPubOrder(author, currentPage, pageSize);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string authorID)
    {
        // filter by the provided author id
        return facade.Get(authorID);
    }

    // public List<CheepViewModel> GetPaginatedCheepsFromAuthor(string authorID, int currentPage = 1)
    // {
    //     return facade.Get32InPubOrder(authorID, currentPage);
    // }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
