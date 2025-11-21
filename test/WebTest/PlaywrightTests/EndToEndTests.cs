using System.Diagnostics;
using System.Text.RegularExpressions;
//using Azure;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

/*
 * Similar to the UI tests above, implement some suitable end-to-end test cases.
 * For example, test if a cheep that a user enters into a cheep box is stored in the
 * database for the respective author.
 */
namespace PlaywrightTests;
[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class EndToEndTests : PageTest
{
    //private Process _serverProcess;

    [SetUp]
    public async Task Init()
    {
        //_serverProcess = await MyEndToEndUtil.StartServer(); // Custom utility class - not part of Playwright
     Page.GotoAsync("https://chirp-ddg2c4bsfsdtewhk.norwayeast-01.azurewebsites.net/");
        Page.SetDefaultTimeout(0); //Azure page is sometimes slow, so make sure the tests doesn't fail due to timeout 
    }
    
    // [TearDown]
    // public async Task Cleanup()
    // {
    //     _serverProcess.Kill();
    //     _serverProcess.Dispose();
    // }
    
    [Ignore("")]
   [Test] 
    public async Task HasTitle()
    {
        //await Page.GotoAsync("https://chirp-ddg2c4bsfsdtewhk.norwayeast-01.azurewebsites.net/");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Chirp!"));
    }
    // Test cases ...

    [Test]
    public async Task DefaultHomePageOnLoadIsPublicCheepPage()
    {
       // await Expect(Page.GetByRole(AriaRole.Heading)).ToHaveTextAsync("Public Timeline");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToHaveTextAsync("Public Timeline");
    }
    
    
}
