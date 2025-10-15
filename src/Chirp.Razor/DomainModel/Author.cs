using System.ComponentModel.DataAnnotations;


namespace Chirp.Razor.DomainModel;

public class Author
{
    public int Id { get; set; }
    
    public required string Username { get; set; }
    
    public required string Email { get; set; }
    
    public required ICollection<Cheep> Cheeps { get; set; }
}