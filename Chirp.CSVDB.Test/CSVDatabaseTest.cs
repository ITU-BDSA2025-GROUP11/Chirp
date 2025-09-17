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
    
    //For example, add a test case that checks that an entry can be received from the database after it was stored in there.
    [Fact]
    public void Read_
    
}