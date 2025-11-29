using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace WebTest;

public class AboutMeUnitTest
{
    CheepRepository _repository;
    ChirpDbContext _context;
    SqliteConnection _connection;
    
    private void Before()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(_connection);

        _context = new ChirpDbContext(builder.Options);
        _context.Database.EnsureCreated();

        _repository = new CheepRepository(_context, NullLoggerFactory.Instance);
    }

    [Fact]
    public async Task DeleteUserAnonymizesDataTest()
    {
        Before();
        var username = "KomUdAfVoresRepoMatthias";
        var email = "matthias@stop";
        await _repository.CreateUser(username, email);
        
        var result = await _repository.DeleteUser(username);
        
        Assert.True(result);
        
        var oldUser = await _context.Authors.FirstOrDefaultAsync(a => a.UserName == username);
        Assert.Null(oldUser);
        
        var anonymizedUser = await _context.Authors.FirstOrDefaultAsync(a => a.Email!.StartsWith("deleted-"));
        Assert.NotNull(anonymizedUser);
        Assert.StartsWith("DeletedUser-", anonymizedUser.UserName);
    }

    [Fact]
    public async Task DeleteUserRemovesFollowRelationsTest()
    {
        Before();
        var userA = "UserA";
        var userB = "UserB";
        await _repository.CreateUser(userA, "a@a.com");
        await _repository.CreateUser(userB, "b@b.com");
        
        var authorA = await _context.Authors.FirstAsync(a => a.UserName == userA);
        var authorB = await _context.Authors.FirstAsync(a => a.UserName == userB);
        
        await _repository.FollowUser(authorA.Id, authorB.Id);
        
        await _repository.DeleteUser(userA);
        
        var deletedUser = await _context.Authors
            .Include(a => a.Following)
            .FirstAsync(a => a.Id == authorA.Id);

        Assert.Empty(deletedUser.Following);
    }
    
    //Edge cases
    [Fact]
    public async Task DeleteUserReturnsFalseWhenUserDoesNotExist()
    {
        Before();

        var result = await _repository.DeleteUser("GivMigDrikkePenge");

        Assert.False(result);
    }
    
    [Fact]
    public async Task GetUserInfoReturnsNullForOldUsernameAfterDeletion()
    {
        Before();

        await _repository.CreateUser("Silas", "Silas@ta.com");
        await _repository.DeleteUser("Silas");

        var info = await _repository.GetUserInfo("Silas");

        Assert.Null(info);
    }
    
    [Fact]
    public async Task DeleteUserRemovesUserFromOtherUsersFollowersList()
    {
        Before();

        await _repository.CreateUser("A", "a@x");
        await _repository.CreateUser("B", "b@x");

        var a = await _context.Authors.FirstAsync(x => x.UserName == "A");
        var b = await _context.Authors.FirstAsync(x => x.UserName == "B");

        await _repository.FollowUser(a.Id, b.Id);

        await _repository.DeleteUser("B");

        var updatedA = await _context.Authors
            .Include(x => x.Following)
            .FirstAsync(x => x.Id == a.Id);

        Assert.False(updatedA.Following.Any(u => u.Id == b.Id));
    }
    
    [Fact]
    public async Task DeleteUserKeepsCheepsButAnonymizesAuthor()
    {
        Before();
        
        await _repository.CreateUser("Rotte", "rotte@mail.com");
        var rotte = await _context.Authors.FirstAsync(a => a.UserName == "Rotte");
        
        _context.Cheeps.Add(new Cheep
        {
            Text = "Rotterne er altid en mer end du tror",
            TimeStamp = DateTime.UtcNow,
            Author = rotte
        });
        await _context.SaveChangesAsync();
        
        await _repository.DeleteUser("Rotte");

        var cheep = await _context.Cheeps
            .Include(c => c.Author)
            .FirstAsync();

        Assert.StartsWith("DeletedUser-", cheep.Author!.UserName);
    }
}