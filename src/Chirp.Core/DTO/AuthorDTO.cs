using Chirp.Core.DomainModel;

namespace Chirp.Core.DTO
{
    public class AuthorDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
        public virtual ICollection<Author> Following { get; set; } = new List<Author>();
        
        public virtual ICollection<Author> Followers { get; set; } = new List<Author>();
    }
}