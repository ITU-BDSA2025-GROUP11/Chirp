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
    private string _username;
    private string _email;
    private string _password;
    private string _baseUrl = "https://localhost:7103";
    
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        };
    }

     private Process? _server;
     
     [OneTimeTearDown]
     public void StopServer()
     {
         _server?.Kill(true);
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

         _server = new Process { StartInfo = psi };
         _server.Start();

         Thread.Sleep(20000); // wait for Kestrel
     }
     
    [SetUp]
    public async Task Init()
    {
        await Page.GotoAsync(_baseUrl);

        _username = Guid.NewGuid().ToString();
        _email = $"user_{Guid.NewGuid()}@test.com";
        _password = "Abc123!";
    }
    
    [Test]
    public async Task DefaultHomePageOnLoadIsPublicCheepPage()
    {
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" }))
            .ToHaveTextAsync("Public Timeline");
    }

    [Test]
    public async Task PressLoginButtonRedirectsToLoginPage()
    {
        await Page.GetByText("Login").ClickAsync();
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
        await RegisterUserTask();

        Console.WriteLine("Successfully created a new user");

        if (await Page.GetByText("Logout").IsVisibleAsync())
            await Page.GetByText("Logout").ClickAsync();

        Console.WriteLine("Logged out");

        await Page.GetByText("Login").ClickAsync();
        await Expect(Page.Locator("#Input_Email")).ToBeVisibleAsync();

        await Page.Locator("#Input_Email").FillAsync(_email);

        await Page.Locator("#Input_Password").FillAsync(_password);

        await Page.Locator("#login-submit").ClickAsync();

        await Expect(Page.GetByText("Logout [")).ToBeVisibleAsync();
    }

    [Ignore(reason: "Broken test")]
    [Test]
    public async Task AddNewCheepDisplaysCheepOnPublicTimeline()
    {
        await RegisterUserTask();
        string testCheep = "Testing cheeping on Chirp!";
        await Page.GetByRole(AriaRole.Textbox).FillAsync(testCheep);
        await Page.GetByRole(AriaRole.Button).And(Page.GetByText("Post")).ClickAsync();

        await Expect(Page.Locator("ul > li")).ToContainTextAsync([_username, testCheep]);
    }

    [Ignore(reason: "Broken test")]
    [Test]
    public async Task AddNewCheepDisplaysCheepOnPrivateTimeline()
    {
        await RegisterUserTask();

        string testCheep = "Testing cheeping on Chirp is visible on private TL!";

        await Page.GetByRole(AriaRole.Textbox).FillAsync(testCheep);
        await Page.GetByRole(AriaRole.Button).And(Page.GetByText("Post")).ClickAsync();

        await Page.GetByRole(AriaRole.Link).And(Page.GetByText("My timeline")).ClickAsync();

        var firstCheep = Page.Locator("ul.cheeps > li").First;

        // await Expect(Page.Locator("ul > li")).ToContainTextAsync([_Username,testCheep,]);
        await Expect(firstCheep).ToContainTextAsync(_username);
        await Expect(firstCheep).ToContainTextAsync(testCheep);
    }
    
    [Ignore(reason: "Broken test")]
    [Test]
    public async Task PostingCheepPersistsAcrossReload()
    {
        await RegisterUserTask();

        var message = "Persistence test";

        await Page.GetByRole(AriaRole.Textbox).FillAsync(message);
        await Page.GetByRole(AriaRole.Button).And(Page.GetByText("Post")).ClickAsync();

        await Page.ReloadAsync();

        await Expect(Page.Locator("ul.cheeps"))
            .ToContainTextAsync(message);
    }

    public async Task RegisterUserTask()
    {
        await Page.GetByText("Register").ClickAsync();

        await Page.GetByLabel("Username").FillAsync(_username);
        await Page.GetByLabel("Email").FillAsync(_email);
        await Page.Locator("#Input_Password").FillAsync(_password);
        await Page.Locator("#Input_ConfirmPassword").FillAsync(_password);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
    }
}