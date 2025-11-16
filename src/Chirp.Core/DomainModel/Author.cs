
using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.DomainModel;

public class Author : IdentityUser
{
   /* public Guid AuthorId { get; set; }
    public required string Name { get; set; }
    
    public required string Email { get; set; }*/
    public required ICollection<Cheep> Cheeps { get; set; }
    
    // In Author.cs
    public ICollection<Author> FollowingList { get; set; } = new List<Author>();

   // public string userID { get; set; } = default!;
}