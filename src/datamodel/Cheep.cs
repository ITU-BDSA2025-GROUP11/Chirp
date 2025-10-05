using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Cheep
{
    public string text { get; set; }
    public DateTime timeStamp { get; set; }
    
    [ForeignKey("username")] //this class is missing its own primary key, e.g. "CheepID", to couple with author of cheep
    public string username { get; set; }
}