using Chirp.Core.DomainModel;
using Chirp.Core.DTOs;
using Xunit;

namespace clientTest;

public class UnitTests
{
    [Fact]
    public void CanSetAndGetAuthorsName()
    {
        var author = new Author
        {
            AuthorId = 1,
            Name = "Jane Doe",
            Email = "jane@example.com",
            Cheeps = new List<Cheep>()
        };
        
        Assert.True(author.AuthorId == 1);
        Assert.Equal("Jane Doe", author.Name);
        Assert.Equal("jane@example.com", author.Email);
        Assert.Equal(new List<Cheep>(), author.Cheeps);
        
    }
    
    [Fact]
    public void CanSetAndGetCheepsName()
    {
        var author = new Author
        {
            AuthorId = 1,
            Name = "Jane Doe",
            Email = "jane@example.com",
            Cheeps = new List<Cheep>()
        };
        
        var cheep = new Cheep
        {
            CheepId = 1,
            Text = "Test test test",
            TimeStamp = new DateTime(2025, 3, 15),
            AuthorId = author.AuthorId,
            Author = author
        };
        
        Assert.True(cheep.CheepId == 1);
        Assert.Equal("Test test test", cheep.Text);
        Assert.Equal(new DateTime(2025, 3, 15),  cheep.TimeStamp);
        Assert.Equal(author.AuthorId, cheep.AuthorId);
        Assert.Equal(author, cheep.Author);
        

    }

    [Fact]
    public void CanCreateDTOs()
    {
        var author = new Author
        {
            AuthorId = 1,
            Name = "Jane Doe",
            Email = "jane@example.com",
            Cheeps = new List<Cheep>()
        };

        var cheep = new Cheep
        {
            CheepId = 1,
            Text = "Test test test",
            TimeStamp = new DateTime(2025, 3, 15),
            AuthorId = author.AuthorId,
            Author = author
        };

        var authorDto = new AuthorDTO
        {
            Username = "Jane Doe", // ← match the actual Name
            Email = "jane@example.com"
        };

        var cheepDto = new CheepDTO
        {
            Text = "Test test test",
            TimeStamp = new DateTime(2025, 3, 15),
            Author = authorDto
        };

        Assert.Equal(cheepDto.Text, EntityToDTO.ToDTO(cheep).Text);
        Assert.Equal(cheepDto.TimeStamp, EntityToDTO.ToDTO(cheep).TimeStamp);
        Assert.Equal(cheepDto.Author.Username, EntityToDTO.ToDTO(cheep).Author.Username);
        Assert.Equal(cheepDto.Author.Email, EntityToDTO.ToDTO(cheep).Author.Email);

        var mappedAuthor = EntityToDTO.ToDTO(author);
        Assert.Equal(authorDto.Username, mappedAuthor.Username);
        Assert.Equal(authorDto.Email, mappedAuthor.Email);
    }
    
    
    
}