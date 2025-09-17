using System.Diagnostics;
using System.Globalization;
using Chirp.CSVDB;
using CsvHelper;

namespace Chirp.CLI.Test;

public class E2E_Test
{
        [Fact]
        public void TestChirp()
        {
            // Arrange
            CSVDatabase<Cheep> cheepDB = new CSVDatabase<Cheep>();
            string testMessage = "MASTER HAS GIVEN DOBBY A SOCK";
            string[] testCLI = new string[] { "chirp", testMessage };
            
            StreamReader reader = new("../chirp_cli_db.csv");
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            // Act
            cheepDB.Cli(testCLI, cheepDB);
            
            var messages = csv.GetRecords<Cheep>().Select(c => c.Message).ToList();

            foreach (var message in messages)
            {
                Console.WriteLine("AAAA");
            }
            
            // Assert
            
            // we assert that last tweet in database is our tweet
            // Assert.StartsWith("ropf", fstCheep);
            // Assert.EndsWith("Hello, World!", fstCheep);
        }
    }