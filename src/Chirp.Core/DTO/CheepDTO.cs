using Chirp.Core.DomainModel;

namespace Chirp.Core.DTO
{
    /// <summary>
    /// Class for configuration of how to transfer Cheep-data from database to main application
    /// </summary>
    public class CheepDTO
    {
        public int Id { get; set; }
        public required string Text { get; set; } 
        public required DateTime TimeStamp { get; set; }
        public required AuthorDTO Author { get; set; }
        
        public ICollection<Author> Likes { get; set; } = new List<Author>();

        public ICollection<Author> Dislikes { get; set; } = new List<Author>();
    }
}