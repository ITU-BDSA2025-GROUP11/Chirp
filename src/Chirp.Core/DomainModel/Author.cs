using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.DomainModel;
/// <summary>
/// Entity class for the concept of Authors and Users in the Chirp program
/// Inherits from IdentityUser
/// </summary>
public class Author : IdentityUser
{
    public required ICollection<Cheep> Cheeps { get; set; }
    
    public virtual ICollection<Author> Following { get; set; } = new List<Author>();
    
    public virtual ICollection<Author> Followers { get; set; } = new List<Author>();
    
    public virtual ICollection<Cheep> LikedCheeps { get; set; } = new List<Cheep>();
    public virtual ICollection<Cheep> DislikedCheeps { get; set; } = new List<Cheep>();
    
    
}