using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

/*
 * Similar to the UI tests above, implement some suitable end-to-end test cases.
 * For example, test if a cheep that a user enters into a cheep box is stored in the
 * database for the respective author.
 */
namespace WebTest;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class EndToEndTest : PageTest
{
    [SetUp]
    public async Task Init()
    {
        
      await Page.GotoAsync("https://chirp-ddg2c4bsfsdtewhk.norwayeast-01.azurewebsites.net/");
       Page.SetDefaultTimeout(20000); //Azure page is sometimes slow, so make sure the tests doesn't fail due to timeout 
    }
    
    [Ignore("reason")]
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
        string username = "TestUser1";
        string email = $"user_{Guid.NewGuid()}@test.com";
        string password = "Abc123!";
        
        await Page.GetByText("Register").ClickAsync();

  
        await Page.GetByLabel("Username").FillAsync(username);
        await Page.GetByLabel("Email").FillAsync(email);
        await Page.Locator("#Input_Password").FillAsync(password);
        await Page.Locator("#Input_ConfirmPassword").FillAsync(password);
        
     
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();

        
        await Expect(Page.GetByText("Public Timeline")).ToBeVisibleAsync();

        
        if (await Page.GetByText("Logout").IsVisibleAsync())
            await Page.GetByText("Logout").ClickAsync();

  
        await Page.GetByText("Login").ClickAsync();

        await Page.GetByLabel("Email").FillAsync(email);
        await Page.GetByLabel("Password").FillAsync(password);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        
        await Expect(Page.GetByText(username)).ToBeVisibleAsync();
    }
}