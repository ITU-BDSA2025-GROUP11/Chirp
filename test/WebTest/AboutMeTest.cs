using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace PagesTest;

public class AboutMeUnitTest
{
    public required CheepRepository cheepRepository;
    public required AuthorRepository authorRepository;
    public required ChirpDbContext Context;
    public required SqliteConnection Connection;

    private void Before()
    {
        Connection = new SqliteConnection("Filename=:memory:");
        Connection.Open();

        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(Connection);

        Context = new ChirpDbContext(builder.Options);
        Context.Database.EnsureCreated();

        authorRepository = new AuthorRepository(Context, NullLoggerFactory.Instance);
        cheepRepository = new CheepRepository(Context, NullLoggerFactory.Instance);
    }

    [Fact]
    public async Task DeleteUserAnonymizesDataTest()
    {
        Before();
        var username = "KomUdAfVoresRepoMatthias";
        var email = "matthias@stop";
        await authorRepository.CreateUser(username, email);
        
        var result = await authorRepository.DeleteUser(username);
        
        Assert.True(result);
        
        var oldUser = await Context.Authors.FirstOrDefaultAsync(a => a.UserName == username);
        Assert.Null(oldUser);
        var anonymizedUser = await Context.Authors.FirstOrDefaultAsync(a => a.Email!.StartsWith("deleted-"));
        Assert.NotNull(anonymizedUser);
        Assert.StartsWith("DeletedUser-", anonymizedUser.UserName);
    }

    [Fact]
    public async Task DeleteUserRemovesFollowRelationsTest()
    {
        Before();
        var userA = "UserA";
        var userB = "UserB";
        await authorRepository.CreateUser(userA, "a@a.com");
        await authorRepository.CreateUser(userB, "b@b.com");
        
        var authorA = await Context.Authors.FirstAsync(a => a.UserName == userA);
        var authorB = await Context.Authors.FirstAsync(a => a.UserName == userB);
        
        await authorRepository.FollowUser(authorA.Id, authorB.Id);
        
        await authorRepository.DeleteUser(userA);
        
        var deletedUser = await Context.Authors
            .Include(a => a.Following)
            .FirstAsync(a => a.Id == authorA.Id);

        Assert.Empty(deletedUser.Following);
    }
    
    //Edge cases
    [Fact]
    public async Task DeleteUserReturnsFalseWhenUserDoesNotExist()
    {
        Before();

        var result = await authorRepository.DeleteUser("GivMigDrikkePenge");

        Assert.False(result);
    }
    
    [Fact]
    public async Task GetUserInfoReturnsNullForOldUsernameAfterDeletion()
    {
        Before();

        await authorRepository.CreateUser("Silas", "Silas@ta.com");
        await authorRepository.DeleteUser("Silas");

        var info = await authorRepository.GetUserInfo("Silas");
        Assert.Null(info);
    }
    
    [Fact]
    public async Task DeleteUserRemovesUserFromOtherUsersFollowersList()
    {
        Before();

        await authorRepository.CreateUser("A", "a@x");
        await authorRepository.CreateUser("B", "b@x");

        var a = await Context.Authors.FirstAsync(x => x.UserName == "A");
        var b = await Context.Authors.FirstAsync(x => x.UserName == "B");

        await authorRepository.FollowUser(a.Id, b.Id);

        await authorRepository.DeleteUser("B");

        var updatedA = await Context.Authors
            .Include(x => x.Following)
            .FirstAsync(x => x.Id == a.Id);
        Assert.DoesNotContain(updatedA.Following, u => u.Id == b.Id);
    }
    
    [Fact]
    public async Task DeleteUserKeepsCheepsButAnonymizesAuthor()
    {
        Before();
        
        await authorRepository.CreateUser("Rotte", "rotte@mail.com");
        var rotte = await Context.Authors.FirstAsync(a => a.UserName == "Rotte");
        
        Context.Cheeps.Add(new Cheep
        {
            Text = "Rotterne er altid en mer end du tror",
            TimeStamp = DateTime.UtcNow,
            Author = rotte
        });
        await Context.SaveChangesAsync();
        
        await authorRepository.DeleteUser("Rotte");

        var cheep = await Context.Cheeps
            .Include(c => c.Author)
            .FirstAsync();

        Assert.StartsWith("DeletedUser-", cheep.Author.UserName);
    }
}