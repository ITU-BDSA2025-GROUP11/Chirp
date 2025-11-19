using Chirp.Core.DomainModel;
using Chirp.Core.DTOs;
using Xunit;

namespace clientTest
{
    public class DomainAndDtoTests
    {
        [Fact]
        public void Author_WhenPropertiesSet_CanRetrieveCorrectValues()
        {
            var author = new Author
            {
                Id = "1",
                UserName = "Jane Doe",
                Email = "jane@example.com",
                Cheeps = new List<Cheep>(),
            };

            Assert.Equal("1", author.Id);
            Assert.Equal("Jane Doe", author.UserName);
            Assert.Equal("jane@example.com", author.Email);
            Assert.Empty(author.Cheeps);
            Assert.Empty(author.Followers);
        }

        [Fact]
        public void Cheep_WhenPropertiesSet_CanRetrieveCorrectValues()
        {
            var author = new Author
            {
                Id = "1",
                UserName = "Jane Doe",
                Email = "jane@example.com",
                Cheeps = new List<Cheep>(),
            };

            var cheep = new Cheep
            {
                CheepId = 1,
                Text = "Test test test",
                TimeStamp = new DateTime(2025, 3, 15),
                Author = author
            };

            Assert.Equal(1, cheep.CheepId);
            Assert.Equal("Test test test", cheep.Text);
            Assert.Equal(new DateTime(2025, 3, 15), cheep.TimeStamp);
            Assert.Equal(author.Id, cheep.Author.Id);
            Assert.Equal(author, cheep.Author);
        }

        [Fact]
        public void EntityToDTO_WhenMappingEntities_ReturnsEquivalentDTOs()
        {
            var author = new Author
            {
                Id = "1",
                UserName = "Jane Doe",
                Email = "jane@example.com",
                Cheeps = new List<Cheep>(),
            };

            var cheep = new Cheep
            {
                CheepId = 1,
                Text = "Test test test",
                TimeStamp = new DateTime(2025, 3, 15),
                Author = author
            };

            var authorDto = new AuthorDTO
            {
                Username = "Jane Doe",
                Email = "jane@example.com"
            };

            var cheepDto = new CheepDTO
            {
                Text = "Test test test",
                TimeStamp = new DateTime(2025, 3, 15),
                Author = authorDto
            };

            var mappedCheep = EntityToDTO.ToDTO(cheep);
            Assert.Equal(cheepDto.Text, mappedCheep.Text);
            Assert.Equal(cheepDto.TimeStamp, mappedCheep.TimeStamp);
            Assert.Equal(cheepDto.Author.Username, mappedCheep.Author.Username);
            Assert.Equal(cheepDto.Author.Email, mappedCheep.Author.Email);

            var mappedAuthor = EntityToDTO.ToDTO(author);
            Assert.Equal(authorDto.Username, mappedAuthor.Username);
            Assert.Equal(authorDto.Email, mappedAuthor.Email);
        }
    }
}
