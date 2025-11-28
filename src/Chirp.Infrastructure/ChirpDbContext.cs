using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class ChirpDbContext : IdentityDbContext<Author>
{
    public DbSet<Cheep> Cheeps { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public ChirpDbContext(DbContextOptions<ChirpDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Author>( b =>
        {
            b.HasIndex(u => u.Email)
                .IsUnique();
            b.HasMany(a => a.Following)
                .WithMany(a => a.Followers)
                .UsingEntity(j => j.ToTable("AuthorFollows"));
            b.ToTable(name: "Authors", schema: null);
        });
    }
}