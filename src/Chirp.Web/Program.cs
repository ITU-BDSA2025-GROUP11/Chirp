using System.Security.Claims;
using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DefaultConnection string not found");

if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("DevConnection");
}
builder.Services.AddDbContext<ChirpDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddDefaultIdentity<Author>(options =>
    {
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.,_@";
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ChirpDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddRazorPages();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICheepService, CheepService>();

var clientid = builder.Configuration["authentication:github:clientId"] ?? Environment.GetEnvironmentVariable("CLIENTID");
var clientsecret = builder.Configuration["authentication:github:clientSecret"] ?? Environment.GetEnvironmentVariable("CLIENTSECRET");

builder.Services.AddAuthentication()
    .AddGitHub(o =>
    {
        o.ClientId = clientid ?? string.Empty;
        o.ClientSecret = clientsecret ?? string.Empty;
        o.CallbackPath = "/signin-github";
        o.Scope.Add("read:user");
        o.Scope.Add("user:email");
        o.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
    });


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
    var context = scope.ServiceProvider.GetRequiredService<ChirpDbContext>();
    context.Database.Migrate();
    DbInitializer.SeedDatabase(context);
}

app.Run();