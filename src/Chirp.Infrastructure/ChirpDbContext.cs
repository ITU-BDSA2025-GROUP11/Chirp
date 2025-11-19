using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class ChirpDbContext : IdentityDbContext<Author>
{ 
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
    public ChirpDbContext(DbContextOptions<ChirpDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This ensures Identity creates its tables correctly
        base.OnModelCreating(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Author>() // EntityTypeBuilder<Author>
            .HasIndex(u => u.Email)
            .IsUnique();
            
        modelBuilder.Entity<Author>() // EntityTypeBuilder<Author>
            .HasIndex(u => u.UserName)
            .IsUnique();
         modelBuilder.Entity<Author>()
             .ToTable(name: "Authors", schema: null);
    }
}