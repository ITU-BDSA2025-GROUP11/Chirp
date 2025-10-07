using System.ComponentModel.DataAnnotations;


namespace Chirp.Razor.DomainModel;

public class Author
{
    [Key]
    public required string username { get; set; }
    
    public required string email { get; set; }
    public required ICollection<Cheep> cheeps { get; set; }
}