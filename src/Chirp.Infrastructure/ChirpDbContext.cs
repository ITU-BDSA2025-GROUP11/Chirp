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
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Author>()
            .HasIndex(u => u.Email)
            .IsUnique();
       
        modelBuilder.Entity<Author>()
            .ToTable(name: "Authors", schema: (string)null);
        
        modelBuilder.Entity<Author>()
            .HasMany(a => a.Following)
            .WithMany(a => a.Followers)
            .UsingEntity(j => j.ToTable("AuthorFollows"));
    }
}