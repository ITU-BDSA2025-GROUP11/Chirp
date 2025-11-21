using Chirp.Core.DomainModel;

namespace Chirp.Core.DTO
{
    public static class EntityToDTO
    {
        public static AuthorDTO ToDTO(Author author)
        {
            if (author.UserName == null || author.Email == null) throw new ArgumentException("Username or email is null");
            return new AuthorDTO
            {
                Id = author.Id,
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