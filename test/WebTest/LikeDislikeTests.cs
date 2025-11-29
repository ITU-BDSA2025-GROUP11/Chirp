using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace PagesTest;

public class LikeDislikeTests
{
    public required CheepRepository CheepRepository;
    public required AuthorRepository AuthorRepository;
    public required ChirpDbContext Context;
    public required SqliteConnection Connection;
    private readonly string _cheepAMessage = "a message";
    private readonly string _authorAName = "a";
    private readonly string _authorAEmail = "a@a.com";
    private readonly string _cheepBMessage  = "b message";
    private readonly string _authorBName = "b";
    private readonly string _authorBEmail = "b@b.com";
    private readonly string _cheepCMessage  = "c message";
    private readonly string _authorCName = "c";
    private readonly string _authorCEmail = "c@c.com";
    
    private Author authorA;
    private Author authorB;
    private Author authorC;
    private Cheep cheepA;
    private Cheep cheepB;
    private Cheep cheepC;
    
    private void Before()
    {
        Connection = new SqliteConnection("Filename=:memory:");
        Connection.Open();

        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(Connection);

        Context = new ChirpDbContext(builder.Options);
        Context.Database.EnsureCreated();

        CheepRepository = new CheepRepository(Context, NullLoggerFactory.Instance);
        AuthorRepository = new AuthorRepository(Context, NullLoggerFactory.Instance);
    }

    private async Task CreateAuthorsAndCheeps()
    {
        // user and cheep a
        await AuthorRepository.CreateUser(_authorAName, _authorAEmail);
        authorA = await Context.Authors.FirstAsync(a => a.UserName == _authorAName);
        await CheepRepository.PostCheep(_cheepAMessage, _authorAName, _authorAEmail);
        cheepA = await Context.Cheeps.FirstAsync(c => c.Author ==  authorA);
        
        // user and cheep b
        await AuthorRepository.CreateUser(_authorBName, _authorBEmail);
        authorB = await Context.Authors.FirstAsync(a => a.UserName == _authorBName);
        await CheepRepository.PostCheep(_cheepBMessage, _authorBName, _authorBEmail);
        cheepB = await Context.Cheeps.FirstAsync(c => c.Author ==  authorB);
        
        // user and cheep c
        await AuthorRepository.CreateUser(_authorCName, _authorCEmail);
        authorC = await Context.Authors.FirstAsync(a => a.UserName == _authorCName);
        await CheepRepository.PostCheep(_cheepCMessage, _authorCName, _authorCEmail);
        cheepC = await Context.Cheeps.FirstAsync(c => c.Author ==  authorC);
        
    }

    [Fact]
    public async Task LikeAddsLikeToCheepAndAuthor()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepRepository.LikePost(authorA.Id, cheepB.CheepId);
        Assert.Contains(authorA, cheepB.Likes);
        Assert.Contains(cheepB, authorA.LikedCheeps);
    }
    [Fact]
    public async Task UnlikeRemovesLikeToCheepAndAuthor()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepRepository.LikePost(authorA.Id, cheepB.CheepId);
        await CheepRepository.RemoveLike(authorA.Id, cheepB.CheepId);
        Assert.DoesNotContain(authorA, cheepB.Likes);
        Assert.DoesNotContain(cheepB, authorA.LikedCheeps);
    }

    [Fact]
    public async Task CountOfLikesIsUpdatedOnLike()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepRepository.LikePost(authorA.Id, cheepB.CheepId);
        Assert.Single(cheepB.Likes);
        Assert.Single(authorA.LikedCheeps);
        await CheepRepository.LikePost(authorC.Id, cheepB.CheepId);
        Assert.Equal(2, cheepB.Likes.Count);
    }

    [Fact]
    public async Task CountOfLikesIsNotUpdatedOnDislike()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepRepository.DislikePost(authorA.Id, cheepB.CheepId);
        Assert.Empty(cheepB.Likes);
        Assert.Empty(authorA.LikedCheeps);
    }
    [Fact]
    public async Task CountOfLikesIsUpdatedOnRemoveLike()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepRepository.LikePost(authorA.Id, cheepB.CheepId);
        Assert.Single(cheepB.Likes);
        Assert.Single(authorA.LikedCheeps);
        await CheepRepository.RemoveLike(authorA.Id, cheepB.CheepId);
        Assert.Empty(cheepB.Likes);
        Assert.Empty(authorA.LikedCheeps);
    }
    [Fact]
    public async Task DislikeAddsDislikeToCheepAndAuthor()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepRepository.DislikePost(authorA.Id, cheepB.CheepId);
        Assert.Contains(authorA, cheepB.Dislikes);
        Assert.Contains(cheepB, authorA.DislikedCheeps);
    }
    [Fact]
    public async Task UnDislikeRemovesDislikeToCheepAndAuthor()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepRepository.DislikePost(authorA.Id, cheepB.CheepId);
        await CheepRepository.RemoveDislike(authorA.Id, cheepB.CheepId);
        Assert.DoesNotContain(authorA, cheepB.Dislikes);
        Assert.DoesNotContain(cheepB, authorA.DislikedCheeps);
    }

    [Fact]
    public async Task CountOfDislikesIsUpdatedOnDislike()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepRepository.DislikePost(authorA.Id, cheepB.CheepId);
        Assert.Single(cheepB.Dislikes);
        Assert.Single(authorA.DislikedCheeps);
        await CheepRepository.DislikePost(authorC.Id, cheepB.CheepId);
        Assert.Equal(2, cheepB.Dislikes.Count);
    }
    
    [Fact]
    public async Task CountOfDislikesIsNotUpdatedOnLike()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepRepository.LikePost(authorA.Id, cheepB.CheepId);
        Assert.Empty(cheepB.Dislikes);
        Assert.Empty(authorA.DislikedCheeps);
    }
    [Fact]
    public async Task CountOfDislikesIsUpdatedOnRemoveDislike()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepRepository.DislikePost(authorA.Id, cheepB.CheepId);
        Assert.Single(cheepB.Dislikes);
        Assert.Single(authorA.DislikedCheeps);
        await CheepRepository.RemoveDislike(authorA.Id, cheepB.CheepId);
        Assert.Empty(cheepB.Dislikes);
        Assert.Empty(authorA.DislikedCheeps);
    }

    [Fact]
    public async Task UserCantLikeOwnPost()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepRepository.DislikePost(authorC.Id, cheepC.CheepId);
        Assert.Empty(cheepC.Likes);
        Assert.Empty(authorC.LikedCheeps);
    }

    [Fact]
    public async Task UserCantDislikeOwnPost()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepRepository.DislikePost(authorC.Id, cheepC.CheepId);
        Assert.Empty(cheepC.Dislikes);
        Assert.Empty(authorC.DislikedCheeps);
    }
    [Fact]
    public async Task UserCantLikePostTwice()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepRepository.LikePost(authorB.Id, cheepC.CheepId);
        await CheepRepository.LikePost(authorB.Id, cheepC.CheepId);
        Assert.Single(cheepC.Likes);
        Assert.Single(authorB.LikedCheeps);
    }

    [Fact]
    public async Task UserCantDislikePostTwice()
    {
        Before();
        await CreateAuthorsAndCheeps();
        await CheepRepository.DislikePost(authorA.Id, cheepC.CheepId);
        await CheepRepository.DislikePost(authorA.Id, cheepC.CheepId);
        Assert.Single(cheepC.Dislikes);
        Assert.Single(authorA.DislikedCheeps);
    }
}