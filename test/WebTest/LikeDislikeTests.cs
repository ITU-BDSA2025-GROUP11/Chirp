using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace WebTest;

public class LikeDislikeTests
{
    public required CheepRepository CheepRepository;
    public required AuthorRepository AuthorRepository;
    public required ChirpDbContext Context;
    public required SqliteConnection Connection;
    public required CheepService CheepService;
    public required AuthorService AuthorService;
    
    private readonly string _authorAName = "a";
    private readonly string _authorAEmail = "a@a.com";
    private readonly string _cheepBMessage  = "b message";
    private readonly string _authorBName = "b";
    private readonly string _authorBEmail = "b@b.com";
    private readonly string _cheepCMessage  = "c message";
    private readonly string _authorCName = "c";
    private readonly string _authorCEmail = "c@c.com";
    
    public required Author AuthorA;
    public required Author AuthorB;
    public required Author AuthorC;
    public required Cheep CheepB;
    public required Cheep CheepC;
    
    private void Before()
    {
        Connection = new SqliteConnection("Filename=:memory:");
        Connection.Open();

        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(Connection);
        Context = new ChirpDbContext(builder.Options);
        Context.Database.EnsureCreated();

        CheepRepository = new CheepRepository(Context, NullLoggerFactory.Instance);
        AuthorRepository = new AuthorRepository(Context, NullLoggerFactory.Instance);
        
        AuthorService = new AuthorService(AuthorRepository, NullLogger<AuthorService>.Instance);
        CheepService = new CheepService(CheepRepository, NullLogger<CheepService>.Instance);
    }

    private async Task CreateAuthorsAndCheeps()
    {
        // user a
        await AuthorService.CreateUser(_authorAName, _authorAEmail);
        AuthorA = await Context.Authors.FirstAsync(a => a.UserName == _authorAName);
        
        // user and cheep b
        await AuthorService.CreateUser(_authorBName, _authorBEmail);
        AuthorB = await Context.Authors.FirstAsync(a => a.UserName == _authorBName);
        await CheepService.PostCheep(_cheepBMessage, AuthorB.Id);
        CheepB = await Context.Cheeps.FirstAsync(c => c.Author ==  AuthorB);
        
        // user and cheep c
        await AuthorService.CreateUser(_authorCName, _authorCEmail);
        AuthorC = await Context.Authors.FirstAsync(a => a.UserName == _authorCName);
        await CheepService.PostCheep(_cheepCMessage, AuthorC.Id);
        CheepC = await Context.Cheeps.FirstAsync(c => c.Author ==  AuthorC);
        
    }

    [Fact]
    public async Task LikeAddsLikeToCheepAndAuthor()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.LikePost(AuthorA.Id, CheepB.CheepId);
        Assert.Contains(AuthorA, CheepB.Likes);
        Assert.Contains(CheepB, AuthorA.LikedCheeps);
    }
    [Fact]
    public async Task UnlikeRemovesLikeToCheepAndAuthor()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.LikePost(AuthorA.Id, CheepB.CheepId);
        await CheepService.RemoveLike(AuthorA.Id, CheepB.CheepId);
        Assert.DoesNotContain(AuthorA, CheepB.Likes);
        Assert.DoesNotContain(CheepB, AuthorA.LikedCheeps);
    }

    [Fact]
    public async Task CountOfLikesIsUpdatedOnLike()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.LikePost(AuthorA.Id, CheepB.CheepId);
        Assert.Single(CheepB.Likes);
        Assert.Single(AuthorA.LikedCheeps);
        await CheepService.LikePost(AuthorC.Id, CheepB.CheepId);
        Assert.Equal(2, CheepB.Likes.Count);
    }

    [Fact]
    public async Task CountOfLikesIsNotUpdatedOnDislike()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.DislikePost(AuthorA.Id, CheepB.CheepId);
        Assert.Empty(CheepB.Likes);
        Assert.Empty(AuthorA.LikedCheeps);
    }
    [Fact]
    public async Task CountOfLikesIsUpdatedOnRemoveLike()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.LikePost(AuthorA.Id, CheepB.CheepId);
        Assert.Single(CheepB.Likes);
        Assert.Single(AuthorA.LikedCheeps);
        await CheepService.RemoveLike(AuthorA.Id, CheepB.CheepId);
        Assert.Empty(CheepB.Likes);
        Assert.Empty(AuthorA.LikedCheeps);
    }
    [Fact]
    public async Task DislikeAddsDislikeToCheepAndAuthor()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.DislikePost(AuthorA.Id, CheepB.CheepId);
        Assert.Contains(AuthorA, CheepB.Dislikes);
        Assert.Contains(CheepB, AuthorA.DislikedCheeps);
    }
    [Fact]
    public async Task UnDislikeRemovesDislikeToCheepAndAuthor()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.DislikePost(AuthorA.Id, CheepB.CheepId);
        await CheepService.RemoveDislike(AuthorA.Id, CheepB.CheepId);
        Assert.DoesNotContain(AuthorA, CheepB.Dislikes);
        Assert.DoesNotContain(CheepB, AuthorA.DislikedCheeps);
    }

    [Fact]
    public async Task CountOfDislikesIsUpdatedOnDislike()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.DislikePost(AuthorA.Id, CheepB.CheepId);
        Assert.Single(CheepB.Dislikes);
        Assert.Single(AuthorA.DislikedCheeps);
        await CheepService.DislikePost(AuthorC.Id, CheepB.CheepId);
        Assert.Equal(2, CheepB.Dislikes.Count);
    }
    
    [Fact]
    public async Task CountOfDislikesIsNotUpdatedOnLike()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.LikePost(AuthorA.Id, CheepB.CheepId);
        Assert.Empty(CheepB.Dislikes);
        Assert.Empty(AuthorA.DislikedCheeps);
    }
    [Fact]
    public async Task CountOfDislikesIsUpdatedOnRemoveDislike()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.DislikePost(AuthorA.Id, CheepB.CheepId);
        Assert.Single(CheepB.Dislikes);
        Assert.Single(AuthorA.DislikedCheeps);
        await CheepService.RemoveDislike(AuthorA.Id, CheepB.CheepId);
        Assert.Empty(CheepB.Dislikes);
        Assert.Empty(AuthorA.DislikedCheeps);
    }

    [Fact]
    public async Task UserCantLikeOwnPost()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.DislikePost(AuthorC.Id, CheepC.CheepId);
        Assert.Empty(CheepC.Likes);
        Assert.Empty(AuthorC.LikedCheeps);
    }

    [Fact]
    public async Task UserCantDislikeOwnPost()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.DislikePost(AuthorC.Id, CheepC.CheepId);
        Assert.Empty(CheepC.Dislikes);
        Assert.Empty(AuthorC.DislikedCheeps);
    }
    [Fact]
    public async Task UserCantLikePostTwice()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.LikePost(AuthorB.Id, CheepC.CheepId);
        await CheepService.LikePost(AuthorB.Id, CheepC.CheepId);
        Assert.Single(CheepC.Likes);
        Assert.Single(AuthorB.LikedCheeps);
    }

    [Fact]
    public async Task UserCantDislikePostTwice()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.DislikePost(AuthorA.Id, CheepC.CheepId);
        await CheepService.DislikePost(AuthorA.Id, CheepC.CheepId);
        Assert.Single(CheepC.Dislikes);
        Assert.Single(AuthorA.DislikedCheeps);
    }

    [Fact]
    public async Task LikeCountIncreasesOnMultipleLikes()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepService.LikePost(AuthorA.Id, CheepC.CheepId);
        await CheepService.LikePost(AuthorB.Id, CheepC.CheepId);
        Assert.Equal(2, CheepC.Likes.Count);
    }
}