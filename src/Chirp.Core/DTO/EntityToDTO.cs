using Chirp.Core.DomainModel;

namespace Chirp.Core.DTOs
{
    public static class EntityToDTO
    {
        public static AuthorDTO ToDTO(Author author)
        {
            return new AuthorDTO
            {
                Id = author.Id ?? string.Empty,
                Username = author.UserName  ?? string.Empty,
                Email = author.Email ?? string.Empty
            };
        }

        public static CheepDTO ToDTO(Cheep cheep)
        {
            return new CheepDTO
            {
                Text = cheep.Text ?? string.Empty,
                TimeStamp = cheep.TimeStamp,
                Author = ToDTO(cheep.Author ?? throw new InvalidOperationException("Cheep must have a author"))
            };
        }
    }
}