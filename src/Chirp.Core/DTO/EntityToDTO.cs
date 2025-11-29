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
                Id = author.Id ?? string.Empty,
                Username = author.UserName  ?? string.Empty,
                Email = author.Email ?? string.Empty,
                Cheeps = author.Cheeps ?? new List<Cheep>(),
                Following = author.Following ?? new List<Author>(),
                Followers = author.Followers ?? new List<Author>(),
                LikedCheeps = author.LikedCheeps ?? new List<Cheep>(),
                DislikedCheeps = author.DislikedCheeps ?? new List<Cheep>()
            };
        }

        public static CheepDTO ToDTO(Cheep cheep)
        {
            return new CheepDTO
            {
                Id = cheep.CheepId,
                Text = cheep.Text ?? string.Empty,
                TimeStamp = cheep.TimeStamp,
                Author = ToDTO(cheep.Author ?? throw new InvalidOperationException("Cheep must have a author")),
                Likes = cheep.Likes,
                Dislikes = cheep.Dislikes,
            };
        }
    }
}