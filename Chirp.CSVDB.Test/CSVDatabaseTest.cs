using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Chirp.CSVDB.Test;

/// <summary>
/// Integration tests for the CSV DB
/// </summary>
public class CSVDatabaseTest : IDisposable
{
    private CSVDatabase<Cheep> cheepDB;
    
    public CSVDatabaseTest() {
        cheepDB = new CSVDatabase<Cheep>();
    }

    public void Dispose()
    {
        cheepDB = null;
    }
}