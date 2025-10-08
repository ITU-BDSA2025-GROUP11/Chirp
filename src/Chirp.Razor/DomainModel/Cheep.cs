using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chirp.Razor.DomainModel;

public class Cheep
{
    public int Id { get; set; }
    
    public string Text { get; set; }
    
    public DateTime TimeStamp { get; set; }
    
    public int AuthorId { get; set; }
    
    [ForeignKey(nameof(AuthorId))]
    public Author Author { get; set; } = null!;
}