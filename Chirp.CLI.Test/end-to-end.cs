using System.Globalization;
using Chirp.CSVDB;
using CsvHelper;


namespace Chirp.CLI.Test;

public class end_to_end
{
    private string testFilePath;
    
    [Fact]
    public void TestChirp()
    {
        // Arrange
        // Creates a new temp CSV file for testing
        testFilePath = Path.Combine(Directory.GetCurrentDirectory(), "test_data.csv");
        
        if (!File.Exists(testFilePath))
        {
            File.WriteAllText(testFilePath,"Author,Message,Timestamp" );
        }
        
        // Initiates a new database
        CSVDatabase<Cheep> cheepDB = new CSVDatabase<Cheep>();
        string testMessage = "MASTER HAS GIVEN DOBBY A SOCK";
        string[] testCLI = new string[] { "chirp", testMessage };
            
        // Act
        cheepDB.Cli(testCLI, cheepDB, testFilePath);
        
        using var sw = new StringWriter();
        Console.SetOut(sw);

        var records = cheepDB.ReadTest(testFilePath).Cast<Cheep>().ToList();
        string consoleOutput = sw.ToString();

        var lastCheep = records.Last();

        Assert.Equal(testMessage, lastCheep.Message);

        Assert.Contains(testMessage, consoleOutput);
        
        // Cleanup
        if (File.Exists(testFilePath))
            File.Delete(testFilePath);
    }
}