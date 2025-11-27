namespace Chirp.Core.DTO
{ 
    public class UserInfoDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<CheepDTO> Cheeps { get; set; } = new List<CheepDTO>();
        public List<string> FollowedUsernames { get; set; } = new List<string>();
    }
}
