using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.DomainModel;

public class ChirpDbContext : DbContext
{
    public ChirpDbContext(DbContextOptions<ChirpDbContext> options)
        : base(options) { }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }
}