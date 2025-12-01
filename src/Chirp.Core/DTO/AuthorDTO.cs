using Chirp.Core.DomainModel;

namespace Chirp.Core.DTO
{
    public class AuthorDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<Cheep>  Cheeps { get; set; } = new List<Cheep>();
        public ICollection<Cheep>  LikedCheeps { get; set; } = new List<Cheep>();
        public ICollection<Cheep>  DislikedCheeps { get; set; } = new List<Cheep>();
        public ICollection<Author> Following { get; set; } = new List<Author>();
        public ICollection<Author> Followers { get; set; } = new List<Author>();
    }
}