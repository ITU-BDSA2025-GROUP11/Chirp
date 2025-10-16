using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DomainModelsTest;

public class CheepTest
{
    [Fact]
    public void MaxLengthOfTextTest()
    {
        // Arrange
        var tooLongString = "a b c d e f g h i j k l m n o p q r s t u v w x y z A B C D E F G H I J K L M N O P Q R S T U V W X Y Z a b c d e f g h i j k l m n o p q r s t u v w x y z A B C D E F G H I J K L M N O P Q R S T U V W X Y Z a b c d e f g h i j k l m n o p q r s t u v w x y z A B C D E F G H I J K L M N O P Q R S T U V W X Y Z a b c d e f g h i j k l m n o p q r s t u v w x y z A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";
        var author = new Author
        {
            AuthorId = 1,
            Cheeps = new List<Cheep>(),
            Email = "test@mail.com",
            Name = "test"
        };
        var newCheep = new Cheep
        {
            Text = tooLongString,
            TimeStamp = DateTime.Now,
            AuthorId = author.AuthorId,
            Author = author
        };
        var optionsBuilder = new DbContextOptionsBuilder<ChirpDbContext>();
        
        optionsBuilder.UseSqlite("Data Source=/Users/milja/3s/bdsa/Chirp/src/Chirp.Web/Chirp.db");
        
        ChirpDbContext dbContext = new ChirpDbContext(optionsBuilder.Options);
        
        // Act
        dbContext.Cheeps.Add(newCheep);
        
        // Assert
        Assert.ThrowsAny<Exception>(() => dbContext.SaveChanges()); // the ef core data decorations only throw exceptions when trying to save conflicting data to the database
        
        
    }
}