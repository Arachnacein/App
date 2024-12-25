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
        
        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenPatternDoesNotExists()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var patternRepository = new PatternRepository(dbContext);

            //act
            var result = await patternRepository.GetAsync(1, Guid.NewGuid());

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenIdIsValidAndUserIdIsInValid()
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
            var result = await patternRepository.GetAsync(1, Guid.Empty);

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenIdIsInValidAndUserIdIsValid()
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
            var result = await patternRepository.GetAsync(21, userId);

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPatterns_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newPatternList = new List<Pattern>
            {
                new Pattern { Id = 1, UserId = userId,  Name = "testPattern", Value_Saves = 30, Value_Entertainment = 30, Value_Fees = 40 },
                new Pattern { Id = 2, UserId = Guid.Empty,  Name = "testPattern2", Value_Saves = 30, Value_Entertainment = 30, Value_Fees = 40 },
                new Pattern { Id = 3, UserId = userId,  Name = "testPattern3", Value_Saves = 30, Value_Entertainment = 30, Value_Fees = 40 }
            };
            dbContext.AddRange(newPatternList);
            await dbContext.SaveChangesAsync();

            var patternRepository = new PatternRepository(dbContext);

            //act
            var result = await patternRepository.GetAllAsync(userId);

            //assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnEmptyList_WhenPatternsWithUserIdNotFound()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newPatternList = new List<Pattern>
            {
                new Pattern { Id = 1, UserId = userId,  Name = "testPattern", Value_Saves = 30, Value_Entertainment = 30, Value_Fees = 40 },
                new Pattern { Id = 2, UserId = Guid.Empty,  Name = "testPattern", Value_Saves = 30, Value_Entertainment = 30, Value_Fees = 40 }
            };
            dbContext.AddRange(newPatternList);
            await dbContext.SaveChangesAsync();

            var patternRepository = new PatternRepository(dbContext);

            //act
            var result = await patternRepository.GetAllAsync(Guid.NewGuid());

            //assert
            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewPattern_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var newPattern = new Pattern
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                Name = "testPattern",
                Value_Saves = 30,
                Value_Entertainment = 30,
                Value_Fees = 40
            };
            var patternRepository = new PatternRepository(dbContext);

            //act
            await patternRepository.AddAsync(newPattern);

            //assert
            dbContext.Patterns.Should().HaveCount(1);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnNewPattern_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var newPattern = new Pattern
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                Name = "testPattern",
                Value_Saves = 30,
                Value_Entertainment = 30,
                Value_Fees = 40
            };
            var patternRepository = new PatternRepository(dbContext);

            //act
            var result = await patternRepository.AddAsync(newPattern);

            //assert
            result.Should().BeOfType<Pattern>();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemovePattern_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newPattern = new Pattern { Id = 1, UserId = userId, Name = "testPattern", Value_Saves = 30, Value_Entertainment = 30, Value_Fees = 40 };
            await dbContext.AddAsync(newPattern);
            await dbContext.SaveChangesAsync();
            var patternRepository = new PatternRepository(dbContext);

            //act
            await patternRepository.DeleteAsync(1, userId);

            //assert
            dbContext.Patterns.Should().HaveCount(0);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotRemovePattern_WhenIdIsInValidAndUserIdIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newPattern = new Pattern { Id = 1, UserId = userId, Name = "testPattern", Value_Saves = 30, Value_Entertainment = 30, Value_Fees = 40 };
            await dbContext.AddAsync(newPattern);
            await dbContext.SaveChangesAsync();
            var patternRepository = new PatternRepository(dbContext);

            //act & assert
            await patternRepository.Invoking(async x => await x.DeleteAsync(2, userId))
                .Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotRemovePattern_WhenIdIsValidAndUserIdIsInValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newPattern = new Pattern { Id = 1, UserId = userId, Name = "testPattern", Value_Saves = 30, Value_Entertainment = 30, Value_Fees = 40 };
            await dbContext.AddAsync(newPattern);
            await dbContext.SaveChangesAsync();
            var patternRepository = new PatternRepository(dbContext);

            //act & assert
            await patternRepository.Invoking(async x => await x.DeleteAsync(1, Guid.Empty))
                .Should()
                .ThrowAsync<ArgumentNullException>();
        }
    }
}