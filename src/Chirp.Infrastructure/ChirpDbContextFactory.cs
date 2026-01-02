using Chirp.Core.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace Chirp.Infrastructure;
/// <summary>
/// Class used in designtime helps EF-core know how to create a ChirpDbContext for migrations
/// </summary>
public class ChirpDbContextFactory : IDesignTimeDbContextFactory<ChirpDbContext>
{
    public ChirpDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ChirpDbContext>();
        
        optionsBuilder.UseSqlite("Data Source=../Chirp.Web/Chirp.db");
        
        return new ChirpDbContext(optionsBuilder.Options);
    }
    
}