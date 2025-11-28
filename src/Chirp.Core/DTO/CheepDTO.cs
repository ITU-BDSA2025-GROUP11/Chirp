using Chirp.Core.DomainModel;

namespace Chirp.Core.DTO
{
    public class CheepDTO
    {
        public string Id { get; set; } = string.Empty;
        public required string Text { get; set; } 
        public required DateTime TimeStamp { get; set; }
        public required AuthorDTO Author { get; set; }
        
        public ICollection<Author> Likes { get; set; } = new List<Author>();

        public ICollection<Author> Dislikes { get; set; } = new List<Author>();
    }
}