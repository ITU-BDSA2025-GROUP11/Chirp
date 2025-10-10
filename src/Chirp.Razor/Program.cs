using Chirp.Razor;
using Chirp.Razor.DomainModel;
using Microsoft.EntityFrameworkCore;

//Region build app and register DbContext
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ChirpDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<ICheepRepository, CheepRepository>();

var app = builder.Build();

//Region MRGA
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var cheepRepo = scope.ServiceProvider.GetRequiredService<ICheepRepository>();
    
    // Post a test cheep
        cheepRepo.PostCheep("Johnny: You betrayed me! You're not good. You, you're just a chicken. Chip-chip-chip-chip-cheep-cheep.");
        cheepRepo.PostCheep("Johnny: No, I can't. Anyway, how is your sex life?");
        cheepRepo.PostCheep("Claudette: I got the results of the test back - I definitely have breast cancer.");
        cheepRepo.PostCheep("Denny: I just like to watch you guys.");
        cheepRepo.PostCheep("Mark: What are you talking about? I just saw you!");
        cheepRepo.PostCheep("Johnny: Don't touch me, motherfucker - geddout.");
}


app.Run();




