using System;
using System.Collections.Generic;
using System.Linq;
using Chirp.Core.DomainModel;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace databasetest
{
    public class UnitTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly ChirpDbContext _context;
        private readonly CheepRepository _repo;

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
            _repo = new CheepRepository(_context, loggerFactory);
        }

        public void Dispose()
        {
            _context?.Dispose();
            _connection?.Dispose();
        }

        private Author CreateTestAuthor(string name = "TestUser", string email = "test@example.com")
        {
            var author = new Author { Name = name, Email = email, Cheeps = new List<Cheep>() };
            _context.Authors.Add(author);
            _context.SaveChanges();
            return author;
        }

        [Fact]
        public void GetCheeps_WhenNoAuthorProvided_ReturnsAllCheeps()
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

            var result = _repo.GetCheeps();

            Assert.Single(result);
            Assert.Equal("Hello World!", result[0].Text);
            Assert.Equal("TestUser", result[0].Author.Username);
        }

        [Fact]
        public void GetCheeps_WhenAuthorNotFound_ReturnsEmptyList()
        {
            var result = _repo.GetCheeps("NonExistentAuthor");
            Assert.Empty(result);
        }

        [Fact]
        public void GetPaginatedCheeps_WhenCalled_ReturnsCorrectSubset()
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

            var page = _repo.GetPaginatedCheeps(currentPage: 0, pageSize: 5);

            Assert.Equal(5, page.Count);
            Assert.Equal("Cheep 0", page[0].Text);
        }

        [Fact]
        public void PostCheep_WhenCalled_AddsCheepForCurrentUser()
        {
            _repo.CreateUser();
            var currentUserName = Environment.UserName;

            _repo.PostCheep("Test Cheep");

            var cheeps = _context.Cheeps.Include(c => c.Author).ToList();
            Assert.Single(cheeps);
            Assert.Equal("Test Cheep", cheeps[0].Text);
            Assert.Equal(currentUserName, cheeps[0].Author.Name);
        }

        [Fact]
        public void CreateUser_WhenCalledTwice_DoesNotDuplicateUser()
        {
            _repo.CreateUser();
            var countAfterFirst = _context.Authors.Count();

            _repo.CreateUser();
            var countAfterSecond = _context.Authors.Count();

            Assert.Equal(countAfterFirst, countAfterSecond);
        }
    }
}
