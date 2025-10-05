using System.ComponentModel.DataAnnotations;

namespace Models;

public class Author
{
    [Key]
    public string userame { get; set; }
    
    public string email { get; set; }
    public ICollection<Cheep> cheeps { get; set; }
}