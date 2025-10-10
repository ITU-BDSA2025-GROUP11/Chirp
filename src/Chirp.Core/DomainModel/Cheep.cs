namespace Chirp.Core.DomainModel;

public class Cheep
{
    public int Id { get; set; }
    
    public string Text { get; set; }
    
    public DateTime TimeStamp { get; set; }
    
    public int AuthorId { get; set; } //foreign key
    public Author Author { get; set; }
    
}