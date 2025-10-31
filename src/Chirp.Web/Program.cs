using Chirp.Core.DomainModel;
using Chirp.Core.DTOs;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;

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
    
    
    
    // var cheepRepo = scope.ServiceProvider.GetRequiredService<ICheepRepository>();
    //
    // // Post a test cheep
    // for (int i = 0; i < 3; i++){
    //     cheepRepo.PostCheep("Johnny: You betrayed me! You're not good. You, you're just a chicken. Chip-chip-chip-chip-cheep-cheep.");
    //     cheepRepo.PostCheep("Johnny: No, I can't. Anyway, how is your sex life?");
    //     cheepRepo.PostCheep("Claudette: I got the results of the test back - I definitely have breast cancer.");
    //     cheepRepo.PostCheep("Denny: I just like to watch you guys.");
    //     cheepRepo.PostCheep("Mark: What are you talking about? I just saw you!");
    //     cheepRepo.PostCheep("Johnny: Don't touch me, motherfucker - geddout.");
    // }
}

app.Run();
