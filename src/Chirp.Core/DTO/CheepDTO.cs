using System;

namespace Chirp.Core.DTO
{
    public class CheepDTO
    {
        public required string Text { get; set; } 
        public required DateTime TimeStamp { get; set; }
        public required AuthorDTO Author { get; set; }
    }
}