using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure;

public class ApplicationUser : IdentityUser
{
    public required Author? Author { get; set; }
}

