using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.DomainModel;

public class Cheep
{
    public int CheepId { get; set; }
    
    [StringLength(160)]
    public string Text { get; set; }
    
    public DateTime TimeStamp { get; set; }
    
    public int AuthorId { get; set; } //foreign key
    public Author Author { get; set; }
    
}