using Chirp.Core.DomainModel;

namespace Chirp.Core.DTO
{
    public static class EntityToDTO
    {
        public static AuthorDTO ToDto(Author author)
        {
            return new AuthorDTO
            {
                Id = author.Id,
                Username = author.UserName,
                Email = author.Email
            };
        }

        public static CheepDTO ToDto(Cheep cheep)
        {
            return new CheepDTO
            {
                Text = cheep.Text,
                TimeStamp = cheep.TimeStamp,
                Author = ToDto(cheep.Author)
            };
        }
    }
}