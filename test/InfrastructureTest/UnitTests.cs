using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace databasetest
{
    public class UnitTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly ChirpDbContext _context;
        private readonly CheepRepository _cheepRepo;
        private readonly AuthorRepository _authorRepo;

        public UnitTests()
        {
            //Create a shared in-memory SQLite connection
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            //Configure DbContext to use it
            var options = new DbContextOptionsBuilder<ChirpDbContext>()
                .UseSqlite(_connection)
                .Options;

            //Create database schema
            _context = new ChirpDbContext(options);
            _context.Database.EnsureCreated();

            //Create repository
            var loggerFactory = LoggerFactory.Create(builder => builder.AddFilter((_, __) => false));
            _cheepRepo = new CheepRepository(_context, loggerFactory);
        }

        public void Dispose()
        {
            _context?.Dispose();
            _connection?.Dispose();
        }

        private Author CreateTestAuthor(string name = "TestUser", string email = "test@example.com")
        {
            var author = new Author { UserName = name, Email = email, Cheeps = new List<Cheep>() };
            _context.Authors.Add(author);
            _context.SaveChanges();
            return author;
        }

        [Fact]
        public async Task GetCheeps_WhenNoAuthorProvided_ReturnsAllCheeps()
        {
            var author = CreateTestAuthor();
            var cheep = new Cheep
            {
                Text = "Hello World!",
                TimeStamp = DateTime.UtcNow,
                Author = author
            };
            _context.Cheeps.Add(cheep);
            _context.SaveChanges();

            var result = await _cheepRepo.GetCheeps();

            Assert.Single(result);
            Assert.Equal("Hello World!", result[0].Text);
            Assert.Equal("TestUser", result[0].Author.Username);
        }

        [Fact]
        public async Task GetCheeps_WhenAuthorNotFound_ReturnsEmptyList()
        {
            var result = await _cheepRepo.GetCheeps("NonExistentAuthor");
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetPaginatedCheeps_WhenCalled_ReturnsCorrectSubset()
        {
            var author = CreateTestAuthor();
            for (int i = 0; i < 10; i++)
            {
                _context.Cheeps.Add(new Cheep
                {
                    Text = $"Cheep {i}",
                    TimeStamp = DateTime.UtcNow.AddMinutes(-i),
                    Author = author
                });
            }
            _context.SaveChanges();

            var page = await _cheepRepo.GetPaginatedCheeps(currentPage: 0, pageSize: 5);

            Assert.Equal(5, page.Count);
            Assert.Equal("Cheep 0", page[0].Text);
        }

        [Fact]
        public async Task PostCheep_WhenCalled_AddsCheepForCurrentUser()
        {
            
            var currentUserName = Environment.UserName;
            await _authorRepo.CreateUser(currentUserName, currentUserName + "@example.com");
            await _cheepRepo.PostCheep("Test Cheep", currentUserName, currentUserName + "@example.com");

            var cheeps = await _context.Cheeps.Include(c => c.Author).ToListAsync();
            Assert.Single(cheeps);
            Assert.Equal("Test Cheep", cheeps[0].Text);
            Assert.Equal(currentUserName, cheeps[0].Author.UserName);
        }

        [Fact]
        public async Task PostCheep_Cheep_length_above_allowed_characters()
        {
            string text = "The quiet river drifted under the moonlit sky, " +
                          "carrying soft ripples that shimmered like silver threads and whispered gentle " +
                          "stories to the silent forest around.";
            
            var currentUserName = Environment.UserName;
            await _authorRepo.CreateUser(currentUserName, currentUserName + "@example.com");
            await _cheepRepo.PostCheep(text, currentUserName, currentUserName + "@example.com");

            var cheeps = await _context.Cheeps.Include(c => c.Author).ToListAsync();
            Assert.Empty(cheeps); 
        }
        
        [Fact]
        public async Task CreateUser_WhenCalledTwice_DoesNotDuplicateUser()
        {
            await _authorRepo.CreateUser("TestUser", "TestUser");
            var countAfterFirst = await _context.Authors.CountAsync();

            await _authorRepo.CreateUser("TestUser", "TestUser");
            var countAfterSecond = await _context.Authors.CountAsync();

            Assert.Equal(countAfterFirst, countAfterSecond);
        }
    }
}
