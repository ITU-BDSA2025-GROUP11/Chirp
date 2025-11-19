using System.Diagnostics;
using Microsoft.Playwright.NUnit;

[Ignore("")]
public class EndToEndTests : PageTest
{
    private Process _serverProcess;

    [SetUp]
    public async Task Init()
    {
        //_serverProcess = await MyEndToEndUtil.StartServer(); // Custom utility class - not part of Playwright
    }

    [TearDown]
    public async Task Cleanup()
    {
        _serverProcess.Kill();
        _serverProcess.Dispose();
    }

    // Test cases ...
}