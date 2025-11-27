using System;
using Chirp.Core.DomainModel;

namespace Chirp.Core.DTOs
{
    public class CheepDTO
    {
        public string Text { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public AuthorDTO Author { get; set; } = new AuthorDTO();
        
        public ICollection<Author> Dislikes { get; set; } = new List<Author>();
        
        public ICollection<Author> Likes { get; set; } = new List<Author>();
        
    }
}