using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class ChirpDbContext : IdentityDbContext<ApplicationUser>
{
    public ChirpDbContext(DbContextOptions<ChirpDbContext> options)
        : base(options) { }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This ensures Identity creates its tables correctly
        base.OnModelCreating(modelBuilder);

        // Add your Author -> IdentityUser relationship mapping:
        modelBuilder.Entity<Author>()
            .HasOne<ApplicationUser>()        // no navigation property needed
            .WithMany()
            .HasForeignKey(a => a.IdentityUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
}
