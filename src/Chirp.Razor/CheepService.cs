using Chirp.Razor.DomainModel;

namespace Chirp.Razor;
using Microsoft.EntityFrameworkCore;

public class CheepService
{
    readonly ChirpDbContext _context;
    readonly ILogger _logger;
    public CheepService(ChirpDbContext context, ILoggerFactory factory)
    {
        _context = context;
        _logger = factory.CreateLogger<CheepService>();
    }