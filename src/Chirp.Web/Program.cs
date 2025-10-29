using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// path
var connectionString = "Data Source=./Chirp.db";  //builder.Configuration.GetConnectionString("DefaultConnection");// path end

builder.Services.AddDbContext<ChirpDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<ICheepRepository, CheepRepository>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ChirpDbContext>();

var app = builder.Build();

//Region MRGA
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization(); 

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{

    
    var db = scope.ServiceProvider.GetRequiredService<ChirpDbContext>();
    // db.Database.Migrate();
    
    try
    {
        Console.WriteLine($"[DEBUG] Database used: {Path.GetFullPath(db.Database.GetDbConnection().DataSource)}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] {ex}");
    }

    var context = scope.ServiceProvider.GetRequiredService<ChirpDbContext>();

    DbInitializer.SeedDatabase(context);
    
}

app.Run();
