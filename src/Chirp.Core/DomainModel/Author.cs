

namespace Chirp.Core.DomainModel;

public class Author
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string Email { get; set; }
    
    public required ICollection<Cheep> Cheeps { get; set; }

   // public string userID { get; set; } = default!;
}