using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.DomainModel;

public class Cheep
{
    public int CheepId { get; set; }
    
    [MaxLength(160)]
    public required string  Text { get; set; }
    public required Author Author { get; set; }
    
    public DateTime TimeStamp { get; set; }
    
}