using System.Diagnostics;
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
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        };
    }

    private Process? _server;
    
    [OneTimeTearDown]
    public void TeardownServer()
    {
        // _server?.Kill();
        _server?.Dispose();
    }

    
    [OneTimeSetUp]
    public void StartServer()
    {
        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "run --project ../../../../../src/Chirp.Web/Chirp.Web.csproj --urls=https://localhost:7103",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        Console.WriteLine(Path.GetFullPath("../../../../../src/Chirp.Web/Chirp.Web.csproj"));


        _server = new Process { StartInfo = psi };
    
        _server.OutputDataReceived += (_, e) => Console.WriteLine("[SERVER OUT] " + e.Data);
        _server.ErrorDataReceived += (_, e) => Console.WriteLine("[SERVER ERR] " + e.Data);

        _server.Start();

        _server.BeginOutputReadLine();
        _server.BeginErrorReadLine();
        
        Thread.Sleep(5000); // wait for it to start
       
    }
  
    [SetUp]
    public async Task Init()
    {
        await Page.GotoAsync("https://localhost:7103/");
    }
    
    
    /* [SetUp]
     public async Task Init()
     {

       //await Page.GotoAsync("https://chirp-ddg2c4bsfsdtewhk.norwayeast-01.azurewebsites.net/");
       await Page.GotoAsync("http://localhost:5273/");
        Page.SetDefaultTimeout(20000); //Azure page is sometimes slow, so make sure the tests doesn't fail due to timeout
     }*/
    
    [Ignore("reason")]
    [Test] 
    public async Task HasTitle()
    {
        //await Page.GotoAsync("https://chirp-ddg2c4bsfsdtewhk.norwayeast-01.azurewebsites.net/");
        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Chirp!"));
    }

    [Test]
    public async Task DefaultHomePageOnLoadIsPublicCheepPage()
    {
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToHaveTextAsync("Public Timeline");
    }

    [Test]
    public async Task PressLoginButtonRedirectsToLoginPage()
    { 
        await Page.GetByText("Login").ClickAsync(); //Click Login-button
        await Expect(Page).ToHaveURLAsync("https://localhost:7103/Identity/Account/Login");
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
        string username = Guid.NewGuid().ToString();
        string email = $"user_{Guid.NewGuid()}@test.com";
        string password = "Abc123!";
        
        await Page.GetByText("Register").ClickAsync();

  
        await Page.GetByLabel("Username").FillAsync(username);
        await Page.GetByLabel("Email").FillAsync(email);
        await Page.Locator("#Input_Password").FillAsync(password);
        await Page.Locator("#Input_ConfirmPassword").FillAsync(password);
        
     
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();

        
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
        
        Console.WriteLine("Successfully created a new user");
        
        if (await Page.GetByText("Logout").IsVisibleAsync())
            await Page.GetByText("Logout").ClickAsync();

        Console.WriteLine("Logged out");
        
        await Page.GetByText("Login").ClickAsync();
        await Expect(Page.Locator("#Input_Email")).ToBeVisibleAsync();
        
        await Page.Locator("#Input_Email").FillAsync(email);
        //await Page.GetByLabel("Email").FillAsync(email);
        await Page.Locator("#Input_Password").FillAsync(password);
        
        
        await Page.Locator("#login-submit").ClickAsync();
        //await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        Console.WriteLine("Logged back in");
        
        await Expect(Page.GetByText("Logout [")).ToBeVisibleAsync();
    }
}