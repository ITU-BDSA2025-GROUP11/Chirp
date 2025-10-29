using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure;

public class ApplicationUser : IdentityUser
{
    
    public Author author { get; set; }
}