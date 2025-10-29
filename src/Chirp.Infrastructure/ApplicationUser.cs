using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure;

public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // Add more custom fields here as needed
}