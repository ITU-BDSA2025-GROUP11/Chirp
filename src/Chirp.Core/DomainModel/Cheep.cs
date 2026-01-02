using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Core.DomainModel;
/// <summary>
/// Entity class for the Concept of Cheeps in the Chirp program
/// </summary>
public class Cheep
{
    [BindProperty]
    public int CheepId { get; set; }
    
    [MaxLength(160)]
    public required string  Text { get; set; }
    public required Author Author { get; set; }
    
    public DateTime TimeStamp { get; set; }
    
    public virtual ICollection<Author> Likes { get; set; } = new List<Author>();

    public virtual ICollection<Author> Dislikes { get; set; } = new List<Author>();
    
}