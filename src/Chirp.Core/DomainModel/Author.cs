using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.DomainModel;

public class Author : IdentityUser
{
    public required ICollection<Cheep> Cheeps { get; set; }
    
    public virtual ICollection<Author> Following { get; set; } = new List<Author>();
    
    public virtual ICollection<Author> Followers { get; set; } = new List<Author>();
    
    
}