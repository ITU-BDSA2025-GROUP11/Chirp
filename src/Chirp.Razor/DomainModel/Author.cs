using System.ComponentModel.DataAnnotations;


namespace Chirp.Razor.DomainModel;

public class Author
{
    [Key]
    public string username { get; set; }
    
    public string email { get; set; }
    public ICollection<Cheep> cheeps { get; set; }
}