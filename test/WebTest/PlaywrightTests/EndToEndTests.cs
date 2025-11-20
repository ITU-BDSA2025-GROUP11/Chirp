using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Assert = NUnit.Framework.Assert;

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
    private Task baseURL;

    [SetUp]
    public async Task Init()
    {
        
        baseURL=  Page.GotoAsync("https://chirp-ddg2c4bsfsdtewhk.norwayeast-01.azurewebsites.net/");
        Page.SetDefaultTimeout(0); //Azure page is sometimes slow, so make sure the tests doesn't fail due to timeout 
    }
    
    [TearDown]
    public async Task Cleanup()
     {
         baseURL.Dispose();
     }
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

    [Test]
    public async Task PressLoginButtonRedirectsToLoginPage()
    { 
        await Page.GetByText("Login").ClickAsync(); //Click Login-button
        await Expect(Page.GetByText("Use a local account to log in.")).ToBeVisibleAsync(); //Check that after clicking, we arrive at login page
    }

    [Test]
    public async Task PressRegisterButtonRedirectsToRegisterPage()
    {
        await Page.GetByText("Register").ClickAsync();
        await Expect(Page.GetByText("Create a new account.")).ToBeVisibleAsync();
    }

    [Test]
    public async Task RegisterNewUserAddsUserToDatabase()
    {
        await Page.GetByText("Register").ClickAsync();
        await Page.GetByLabel("Email").FillAsync("John@gmail.com");
        await Page.GetByText("Password").FillAsync("Abc123!");
        await Page.GetByText("Confirm Password").FillAsync("Abc123!");
       // await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        
       var response = await Page.RunAndWaitForResponseAsync(
           async () =>
           {
               await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
           },
           r => r.Url.Contains("/Identity/Account/Register") && r.Status >= 200
       );

       //C# test syntax:
       Assert.Equals(201, response.Status);
    }
}
