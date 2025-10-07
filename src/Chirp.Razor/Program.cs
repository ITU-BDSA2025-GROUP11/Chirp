using Chirp.Razor.DomainModel;
using Microsoft.EntityFrameworkCore;

//Region build app and register DbContext
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ChirpDbContext>(options => options.UseSqlite(connectionString));

var app = builder.Build();


//Region MRGA
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();


