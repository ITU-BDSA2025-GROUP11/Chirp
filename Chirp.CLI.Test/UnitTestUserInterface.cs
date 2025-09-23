using Chirp.CSVDB;

namespace Chirp.CLI.Test;

public class UnitTestUserInterface
{
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
            UserInterface.PrintCheeps(new List<Cheep>{cheep});
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
            UserInterface.PrintCheeps(new List<Cheep>{cheep});
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

        var date = UserInterface.Epoch2dateString(timestamp);

        Assert.Equal("01-01-1970", date);
    }

    [Fact]
    //Time is 2 hours behind the actual time in denmark
    public void Epoch2timeString_ConvertsCorrectly()
    {
        
        long timestamp = 1758091047;
        
        var time = UserInterface.Epoch2timeString(timestamp);
        
        Assert.Equal("06:37:27", time);
    }
    //end region 
}