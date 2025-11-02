using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class ChirpDbContext(DbContextOptions<ChirpDbContext> options) : IdentityDbContext<Author>(options)
{ 
    public DbSet<Cheep> Cheeps { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This ensures Identity creates its tables correctly
        base.OnModelCreating(modelBuilder);
       
    }
}