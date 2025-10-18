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
        
    }
    
    [Fact]
    public void CanSetAndGetCheepsName()
    {
        
    }
    
    [Fact]
    public void CanCreateDTOs()
    {
        
    }
    
    
}