using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
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
    context.Database.EnsureCreated();
    //context.Database.Migrate();
    DbInitializer.SeedDatabase(context);
}

app.Run();
