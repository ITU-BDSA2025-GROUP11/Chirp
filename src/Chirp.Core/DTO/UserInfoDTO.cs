namespace Chirp.Core.DTO
{ 
    /// <summary>
    /// Class for configuration of how to transfer UserInfo-data from database to main application
    /// </summary>
    public class UserInfoDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<CheepDTO> Cheeps { get; set; } = new();
        public List<string> FollowedUsernames { get; set; } = new();
    }
}
