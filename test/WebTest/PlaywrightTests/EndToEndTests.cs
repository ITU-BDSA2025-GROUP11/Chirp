using System.Diagnostics;
using System.Text.RegularExpressions;
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
    private Process _serverProcess;

   /* [SetUp]
    public async Task Init()
    {
        //_serverProcess = await MyEndToEndUtil.StartServer(); // Custom utility class - not part of Playwright
    }*/

   /* [TearDown]
    public async Task Cleanup()
    {
        _serverProcess.Kill();
        _serverProcess.Dispose();
    }*/
   [Test] 
    public async Task HasTitle()
    {
        await Page.GotoAsync("https://chirp-ddg2c4bsfsdtewhk.norwayeast-01.azurewebsites.net/");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Chirp!"));
    }
    // Test cases ...
}
