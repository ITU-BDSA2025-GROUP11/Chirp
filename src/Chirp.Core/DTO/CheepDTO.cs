using System;

namespace Chirp.Core.DTOs
{
    public class CheepDTO
    {
        public string Text { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public AuthorDTO Author { get; set; } = new AuthorDTO();
    }
}