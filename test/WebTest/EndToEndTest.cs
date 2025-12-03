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
    private string _Username;
    private string _Email;
    private string _Password;
    private string BaseUrl = "https://localhost:7103";
    
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        };
    }

    // private Process? _server;
    //
    // [OneTimeTearDown]
    // public void TeardownServer()
    // {
    //     // _server?.Kill();
    //     _server?.Dispose();
    // }


    // [OneTimeSetUp]
    // public void StartServer()
    // {
    //     var psi = new ProcessStartInfo
    //     {
    //         FileName = "dotnet",
    //         Arguments = "run --project ../../../../../src/Chirp.Web/Chirp.Web.csproj --urls=https://localhost:7103",
    //         RedirectStandardOutput = true,
    //         RedirectStandardError = true,
    //         UseShellExecute = false
    //     };
    //     Console.WriteLine(Path.GetFullPath("../../../../../src/Chirp.Web/Chirp.Web.csproj"));
    //
    //
    //     _server = new Process { StartInfo = psi };
    //
    //     _server.OutputDataReceived += (_, e) => Console.WriteLine("[SERVER OUT] " + e.Data);
    //     _server.ErrorDataReceived += (_, e) => Console.WriteLine("[SERVER ERR] " + e.Data);
    //
    //     _server.Start();
    //
    //     _server.BeginOutputReadLine();
    //     _server.BeginErrorReadLine();
    //
    //     Thread.Sleep(25000); // wait for it to start
    // }

    [SetUp]
    public async Task Init()
    {
        await Page.GotoAsync(BaseUrl);

        _Username = Guid.NewGuid().ToString();
        _Email = $"user_{Guid.NewGuid()}@test.com";
        _Password = "Abc123!";
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
        await RegisterUserTask();

        Console.WriteLine("Successfully created a new user");

        if (await Page.GetByText("Logout").IsVisibleAsync())
            await Page.GetByText("Logout").ClickAsync();

        Console.WriteLine("Logged out");

        await Page.GetByText("Login").ClickAsync();
        await Expect(Page.Locator("#Input_Email")).ToBeVisibleAsync();

        await Page.Locator("#Input_Email").FillAsync(_Email);

        await Page.Locator("#Input_Password").FillAsync(_Password);

        await Page.Locator("#login-submit").ClickAsync();

        await Expect(Page.GetByText("Logout [")).ToBeVisibleAsync();
    }

    [Test]
    public async Task AddNewCheepDisplaysCheepOnPublicTimeline()
    {
        await RegisterUserTask();
        string testCheep = "Testing cheeping on Chirp!";
        await Page.GetByRole(AriaRole.Textbox).FillAsync(testCheep);
        await Page.GetByRole(AriaRole.Button).And(Page.GetByText("Post")).ClickAsync();

        await Expect(Page.Locator("ul > li")).ToContainTextAsync([_Username, testCheep]);
    }

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
        await Expect(firstCheep).ToContainTextAsync(_Username);
        await Expect(firstCheep).ToContainTextAsync(testCheep);
    }

    public async Task RegisterUserTask()
    {
        await Page.GetByText("Register").ClickAsync();

        await Page.GetByLabel("Username").FillAsync(_Username);
        await Page.GetByLabel("Email").FillAsync(_Email);
        await Page.Locator("#Input_Password").FillAsync(_Password);
        await Page.Locator("#Input_ConfirmPassword").FillAsync(_Password);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
    }
}