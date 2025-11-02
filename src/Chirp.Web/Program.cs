using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultIdentity<Author>(options =>
    {
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzæøåABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ0123456789-.,_";
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ChirpDbContext>();

builder.Services.AddRazorPages();

var connectionString = "Data Source=./Chirp.db";  //builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ChirpDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<ICheepRepository, CheepRepository>();

var app = builder.Build();

//Region MRGA
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();
//app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    
    var db = scope.ServiceProvider.GetRequiredService<ChirpDbContext>();
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
