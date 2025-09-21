namespace Chirp.CLI.Test;

using Chirp.CSVDB;
using System.Diagnostics;

public class E2E
{
    [Fact]
    public void TestChirp()
    {

        // ARRANGE
        var process = new Process();
        process.StartInfo.FileName = "dotnet";
        process.StartInfo.Arguments = "run --project /Users/morty/RiderProjects/Chirp/CsvDbService.Test/CsvDbService.Test.csproj";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        // process.OutputDataReceived += (_, e) => Console.WriteLine(e.Data);
        // process.ErrorDataReceived += (_, e) => Console.WriteLine("ERR: " + e.Data);

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        Thread.Sleep(10000);
        
        CSVDatabase<Cheep> cheepDB = new CSVDatabase<Cheep>();
        cheepDB.setUrl("http://localhost:5001");
        cheepDB.setpath("/Users/morty/RiderProjects/Chirp/CsvDbService.Test/test_data.csv");
        
        string testMessage = "MASTER HAS GIVEN DOBBY A SOCK";
        string[] chirpCLI = new string[] { "chirp", testMessage };
        string[] printCLI = new string[] { "print"};
        
        using var sw = new StringWriter();
        
        // Act
        cheepDB.Cli(chirpCLI, cheepDB);
        Thread.Sleep(5000);
        Console.SetOut(sw);
        cheepDB.Cli(printCLI, cheepDB);
        
        // Assert
        string output = sw.ToString();
        Assert.Contains(testMessage, output);
        
        // Cleanup
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        
        if (!process.HasExited)
        {
            process.Kill(entireProcessTree: true);
            process.WaitForExit();
        }
        process.Dispose();
        
        Console.WriteLine("Test completed");
    }
}