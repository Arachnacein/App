using BudgetManager.Data;
using BudgetManager.Dto;
using BudgetManager.Models;
using BudgetManager.Repositories;
using Castle.Components.DictionaryAdapter.Xml;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace RepositoriesTests
{
    public class MonthPatternRepositoryTests
    {
        private BudgetDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<BudgetDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new BudgetDbContext(options);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnMonthPattern_WhenIdAndUserIdAreValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPattern = new MonthPattern 
            { 
                Id = 1, 
                UserId= userId, 
                Date = DateTime.Now,
                PatternId = 1
            };
            dbContext.Add(newMonthPattern);
            await dbContext.SaveChangesAsync();

            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act
            var result = await monthPatternRepository.GetAsync(1, userId);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<MonthPattern>();
            result.Should().Be(newMonthPattern);

        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenMonthPatternDoesntExists()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act
            var result = await monthPatternRepository.GetAsync(1, Guid.NewGuid());

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenIdIsValidAndUserIdIsInValid()
        {
            //assert
            var dbContext = CreateInMemoryDbContext();
            var newMonthPattern = new MonthPattern
            {
                Id = 1,
                UserId = Guid.Empty,
                Date = DateTime.Now,
                PatternId = 1
            };
            dbContext.Add(newMonthPattern);
            await dbContext.SaveChangesAsync();

            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act
            var result = await monthPatternRepository.GetAsync(1, Guid.NewGuid());

            //assert
            result.Should().BeNull();

        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenIdIsInValidAndUserIdIsValid()
        {
            //assert
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPattern = new MonthPattern
            {
                Id = 1,
                UserId = userId,
                Date = DateTime.Now,
                PatternId = 1
            };
            dbContext.Add(newMonthPattern);
            await dbContext.SaveChangesAsync();

            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act
            var result = await monthPatternRepository.GetAsync(2, userId);

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllMonthPatterns_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPatternList = new List<MonthPattern>
            {
                new MonthPattern { Id = 1, UserId = userId,Date = DateTime.Now, PatternId = 1 },
                new MonthPattern { Id = 2, UserId = userId,Date = DateTime.Now, PatternId = 2 },
                new MonthPattern { Id = 3, UserId = Guid.Empty, Date = DateTime.Now, PatternId = 2 }
            };
            dbContext.AddRange(newMonthPatternList);
            await dbContext.SaveChangesAsync();

            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act
            var result = await monthPatternRepository.GetAllAsync(userId);

            //assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenMonthPatternsWithUserIdNotFound()
        {  
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPatternList = new List<MonthPattern>
            {
                new MonthPattern { Id = 1, UserId = userId, Date = DateTime.Now, PatternId = 1 },
                new MonthPattern { Id = 2, UserId = userId, Date = DateTime.Now, PatternId = 2 }
            };
            dbContext.AddRange(newMonthPatternList);
            await dbContext.SaveChangesAsync();

            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act
            var result = await monthPatternRepository.GetAllAsync(Guid.Empty);

            //assert
            result.Should().HaveCount(0);
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewMonthpattern_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPattern = new MonthPattern { Id = 1, Date = DateTime.Now, UserId = userId, PatternId = 2 };
            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act
            await monthPatternRepository.AddAsync(newMonthPattern);

            //assert
            dbContext.MonthPatterns.Should().HaveCount(1);
            dbContext.MonthPatterns.First().Should().BeEquivalentTo(newMonthPattern);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnNewMonthPattern_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPattern = new MonthPattern { Id = 1, Date = DateTime.Now, UserId = userId, PatternId = 2 };
            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act
            var result = await monthPatternRepository.AddAsync(newMonthPattern);

            //assert
            result.Should().BeOfType<MonthPattern>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateMonthPattern_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPattern = new MonthPattern { Id = 1, Date = DateTime.Now, UserId = userId, PatternId = 2 };
            await dbContext.AddAsync(newMonthPattern);
            await dbContext.SaveChangesAsync();
            var monthPatternRepository = new MonthPatternRepository(dbContext);
            
            var preciseTime = DateTime.Now.AddDays(1);
            newMonthPattern.Date = preciseTime;
            newMonthPattern.UserId = Guid.Empty;

            //act
            await monthPatternRepository.UpdateAsync(newMonthPattern);

            //assert
            var monthPattern = await dbContext.MonthPatterns.FindAsync(newMonthPattern.Id);
            monthPattern.Should().NotBeNull();
            monthPattern.Date.Should().Be(preciseTime);
            monthPattern.UserId.Should().Be(Guid.Empty);
        }

        [Fact]
        public async Task UpdateAsync_ShouldNotUpdateMonthPattern_WhenIdIsInvalid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPattern = new MonthPattern { Id = 1, Date = DateTime.Now, UserId = userId, PatternId = 2 };
            var newMonthPattern2 = new MonthPattern { Id = 2, Date = DateTime.Now, UserId = userId, PatternId = 2 };
            await dbContext.AddAsync(newMonthPattern);
            await dbContext.SaveChangesAsync();
            var monthPatternRepository = new MonthPatternRepository(dbContext);


            //act & assert
            await monthPatternRepository
                .Invoking(async x => await x.UpdateAsync(newMonthPattern2))
                .Should()
                .ThrowAsync<DbUpdateConcurrencyException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveMonthPattern_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPattern = new MonthPattern { Id = 1, Date = DateTime.Now, UserId = userId, PatternId = 2 };
            await dbContext.AddAsync(newMonthPattern);
            await dbContext.SaveChangesAsync();
            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act

            await monthPatternRepository.DeleteAsync(newMonthPattern);

            //assert
            dbContext.MonthPatterns.Count().Should().Be(0);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotRemove_WhenMonthPatternNotFound()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var monthPatternRepository = new MonthPatternRepository(dbContext);
            var newMonthPattern = new MonthPattern { Id = 1, Date = DateTime.Now, UserId= Guid.NewGuid(), PatternId = 1};
            //act & assert

            await monthPatternRepository
                    .Invoking(async x => await x.DeleteAsync(newMonthPattern))
                    .Should()
                    .ThrowAsync<DbUpdateConcurrencyException>();
        }

        [Fact]
        public async Task GetAsync2_ShouldReturnMonthPattern_WhenMonthYearModelAndUserIdAreValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPattern = new MonthPattern
            {
                Id = 1,
                UserId = userId,
                Date = new DateTime(2024, 8, 13),
                PatternId = 1
            };
            dbContext.Add(newMonthPattern);
            await dbContext.SaveChangesAsync();
            var monthYearModel = new MonthYearModel { Year = 2024, Month = 8 };
            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act
            var result = await monthPatternRepository.GetAsync(monthYearModel, userId);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<MonthPattern>();
            result.UserId.Should().Be(userId);
            result.Date.Should().Be(new DateTime(2024, 8, 13));
            result.Id.Should().Be(1);
            result.PatternId.Should().Be(1);
        }

        [Fact]
        public async Task GetAsync2_ShouldReturnNull_WhenMonthPatternDoesntExists()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPattern = new MonthPattern
            {
                Id = 1,
                UserId = userId,
                Date = new DateTime(2024, 8, 13),
                PatternId = 1
            };
            dbContext.Add(newMonthPattern);
            await dbContext.SaveChangesAsync();
            var monthYearModel = new MonthYearModel { Year = 2024, Month = 12 };
            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act
            var result = await monthPatternRepository.GetAsync(monthYearModel, userId);

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync2_ShouldReturnNull_WhenUserIdIsNotValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPattern = new MonthPattern
            {
                Id = 1,
                UserId = userId,
                Date = new DateTime(2024, 8, 13),
                PatternId = 1
            };
            dbContext.Add(newMonthPattern);
            await dbContext.SaveChangesAsync();
            var monthYearModel = new MonthYearModel { Year = 2024, Month = 8 };
            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act
            var result = await monthPatternRepository.GetAsync(monthYearModel, Guid.Empty);

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CheckExistsAsync_ShouldReturnCount_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPatternList = new List<MonthPattern>
            {
                new MonthPattern { Id = 1, UserId = userId, Date = new DateTime(2024, 8, 12), PatternId = 1 },
                new MonthPattern { Id = 2, UserId = userId, Date = new DateTime(2024, 8, 12), PatternId = 2 },
                new MonthPattern { Id = 3, UserId = userId, Date = new DateTime(2024, 5, 12), PatternId = 2 }
            };
            dbContext.AddRange(newMonthPatternList);
            await dbContext.SaveChangesAsync();
            var monthYearModel = new MonthYearModel { Year = 2024, Month = 8 };
            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act
            var result = await monthPatternRepository.CheckExistsAsync(monthYearModel, userId);

            //assert
            result.Should().Be(2);
        }

        [Fact]
        public async Task CheckExistsAsync_ShouldReturnZero_WhenUserIdIsNotValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPatternList = new List<MonthPattern>
            {
                new MonthPattern { Id = 1, UserId = userId, Date = new DateTime(2024, 8, 12), PatternId = 1 },
                new MonthPattern { Id = 2, UserId = userId, Date = new DateTime(2024, 8, 12), PatternId = 2 },
                new MonthPattern { Id = 3, UserId = userId, Date = new DateTime(2024, 5, 12), PatternId = 2 }
            };
            dbContext.AddRange(newMonthPatternList);
            await dbContext.SaveChangesAsync();
            var monthYearModel = new MonthYearModel { Year = 2024, Month = 8 };
            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act
            var result = await monthPatternRepository.CheckExistsAsync(monthYearModel, Guid.Empty);

            //assert
            result.Should().Be(0);
        }

        [Fact]
        public async Task CheckExistsAsync_ShouldReturnInt_WhenCalled()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newMonthPatternList = new List<MonthPattern>
            {
                new MonthPattern { Id = 1, UserId = userId, Date = new DateTime(2024, 8, 12), PatternId = 1 },
                new MonthPattern { Id = 2, UserId = userId, Date = new DateTime(2024, 8, 12), PatternId = 2 },
                new MonthPattern { Id = 3, UserId = userId, Date = new DateTime(2024, 5, 12), PatternId = 2 }
            };
            dbContext.AddRange(newMonthPatternList);
            await dbContext.SaveChangesAsync();
            var monthYearModel = new MonthYearModel { Year = 2024, Month = 8 };
            var monthPatternRepository = new MonthPatternRepository(dbContext);

            //act
            var result = await monthPatternRepository.CheckExistsAsync(monthYearModel, userId);

            //assert
            result.GetComponentType().Should().Be(typeof(int));
        }
    }
}