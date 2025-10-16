using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Chirp.Infrastructure;

public class ChirpDbContextFactory : IDesignTimeDbContextFactory<ChirpDbContext>
{
    public ChirpDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ChirpDbContext>();
        
        optionsBuilder.UseSqlite("Data Source=/Users/milja/3s/bdsa/Chirp/src/Chirp.Web/Chirp.db");
        
        return new ChirpDbContext(optionsBuilder.Options);
    }
    
}