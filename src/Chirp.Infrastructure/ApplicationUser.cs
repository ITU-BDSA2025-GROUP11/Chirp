using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure;

public class ApplicationUser : IdentityUser
{
    private Author author { get; set}
}