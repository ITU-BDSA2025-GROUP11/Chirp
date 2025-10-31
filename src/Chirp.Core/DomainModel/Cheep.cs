using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.DomainModel;

public class Cheep
{
    public int CheepId { get; set; }
    
    [MaxLength(160)]
    public required string  Text { get; set; }
    
    public DateTime TimeStamp { get; set; }
    
    public string AuthorId { get; set; } //foreign key
    public required Author Author { get; set; }
    
}