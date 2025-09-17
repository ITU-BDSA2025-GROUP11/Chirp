using System;
using System.IO;
using System.Linq;
using Chirp.CSVDB;
using Xunit;

namespace Chirp.CSVDB.Test;

public class UnitTestSCVDatabase
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
    
    //Region formating
    //Formatting when posting cheeps
    [Fact]
    public void FormattingTest()
    {
        var cheep = new Cheep("TestUser", "Test", 1758051416);
        var originalOut = Console.Out; 
        using var sw = new StringWriter();
        Console.SetOut(sw);

        try
        {
            CSVDatabase<Cheep>.PrintCheeps(new List<Cheep>{cheep});
            string output = sw.ToString();
            
            var expectedDate = new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc).AddSeconds(cheep.Timestamp).ToShortDateString();
            var expectedTime = new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc).AddSeconds(cheep.Timestamp).ToLongTimeString();
            var expectedOutput = $"{cheep.Author} @ {expectedDate} {expectedTime}: {cheep.Message}";
            Assert.Contains(expectedOutput, output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
    
    [Fact]
    //Tests if formatting is kept in case of an empty message of author.
    public void FormatingEmptyString()
    {
        var cheep = new Cheep("", "", 1758051416);
        var originalOut = Console.Out; 
        using var sw = new StringWriter();
        Console.SetOut(sw);

        try
        {
            CSVDatabase<Cheep>.PrintCheeps(new List<Cheep>{cheep});
            string output = sw.ToString();
            
            var expectedDate = new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc).AddSeconds(cheep.Timestamp).ToShortDateString();
            var expectedTime = new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc).AddSeconds(cheep.Timestamp).ToLongTimeString();
            var expectedOutput = $" @ {expectedDate} {expectedTime}: ";
            Assert.Contains(expectedOutput, output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
    
    //region end
    
    //region DateTime
    
    [Fact]
    public void Epoch2dateString_ConvertsCorrectly()
    {
        long timestamp = 0;

        var date = CSVDatabase<Cheep>.Epoch2dateString(timestamp);

        Assert.Equal("01-01-1970", date);
    }

    [Fact]
    //Time is 2 hours behind the actual time in denmark
    public void Epoch2timeString_ConvertsCorrectly()
    {
        
        long timestamp = 1758091047;
        
        var time = CSVDatabase<Cheep>.Epoch2timeString(timestamp);
        
        Assert.Equal("06:37:27", time);
    }
}