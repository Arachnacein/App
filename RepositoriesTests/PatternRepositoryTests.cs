using BudgetManager.Data;
using BudgetManager.Models;
using BudgetManager.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace RepositoriesTests
{
    public class PatternRepositoryTests
    {
        private BudgetDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<BudgetDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new BudgetDbContext(options);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnPattern_WhenIdAndUserIdAreValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newPattern = new Pattern
            {
                Id = 1,
                UserId = userId,
                Name = "testPattern",
                Value_Saves = 30,
                Value_Entertainment = 30,
                Value_Fees = 40
            };
            dbContext.Add(newPattern);
            await dbContext.SaveChangesAsync();

            var patternRepository = new PatternRepository(dbContext);

            //act
            var result = await patternRepository.GetAsync(1, userId);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Pattern>();
            result.Should().Be(newPattern);
        }
    }
}