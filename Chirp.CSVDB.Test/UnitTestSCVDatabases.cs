using System;
using System.IO;
using System.Linq;
using Chirp.CSVDB;
using Xunit;

namespace Chirp.CSVDB.Test;

public class UnitTestSCVDatabases
{
    //Region Store and Read
    [Fact]
    //tests Store and Read functions from CSVDatabase
    public void TestingStoreAndRead()
    {
        var tempFile = Path.GetTempFileName();
        var database = new CSVDatabase<Cheep>(tempFile);
        File.WriteAllText(tempFile, "Author,Message,Timestamp\n");
        var cheep = new Cheep("Test_Author", "Test Cheep", 1758051416);
        var cheep1 = new Cheep("...", "...", 1758051416);
        
        
        database.Store(cheep);
        database.Store(cheep1);
        var cheeps = database.Read().ToList();
        
        Assert.Equal(2, cheeps.Count);
        Assert.Equal("Test_Author", cheeps[0].Author);
        Assert.Equal("Test Cheep", cheeps[0].Message);
        Assert.Equal(1758051416, cheeps[0].Timestamp);
    }
    
    [Fact]
    //Tests if read returns empty with only a header and no cheeps
    public void EmptyRead()
    {
        var tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, "Author,Message,Timestamp\n");
    
        var database = new CSVDatabase<Cheep>(tempFile);
        var cheeps = database.Read().ToList();
    
        Assert.Empty(cheeps);
    }

    [Fact]
    //Test to see if read and store handles special characters in author name and message
    public void SpecialCharacters()
    {
        var tempFile = Path.GetTempFileName();
        var database = new CSVDatabase<Cheep>(tempFile);
        File.WriteAllText(tempFile, "Author,Message,Timestamp\n");
        var cheep = new Cheep("Test_Author,.!?ÆØÅäöü漢<>()[]{} test", ",.!?ÆØÅäöü漢<>()[]{}\n test", 1758051416);
        
        database.Store(cheep);
        var cheeps = database.Read().ToList();
        
        Assert.Equal("Test_Author,.!?ÆØÅäöü漢<>()[]{} test", cheeps[0].Author);
        Assert.Equal(",.!?ÆØÅäöü漢<>()[]{}\n test", cheeps[0].Message);
        Assert.Equal(1758051416, cheeps[0].Timestamp);
    }

    //Region end
    

    
    //Error handeling 
    //Tests if code handles a missing file without crashing. 
    [Fact]
    public void ReadMissingFile()
    {
        var missingFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv");
        var database = new CSVDatabase<Cheep>(missingFile);

        var cheeps = database.Read().ToList();

        Assert.Empty(cheeps);
    }
    //Region end 
}