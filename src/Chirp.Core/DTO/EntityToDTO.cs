using Chirp.Core.DomainModel;

namespace Chirp.Core.DTO
{
    public static class EntityToDTO
    {
        public static AuthorDTO ToDTO(Author author)
        {
            return new AuthorDTO
            {
                Username = author.UserName,
                Email = author.Email
            };
        }

        public static CheepDTO ToDTO(Cheep cheep)
        {
            return new CheepDTO
            {
                Text = cheep.Text,
                TimeStamp = cheep.TimeStamp,
                Author = ToDTO(cheep.Author)
            };
        }
    }
}