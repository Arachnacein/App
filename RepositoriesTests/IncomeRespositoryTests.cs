using BudgetManager.Data;
using BudgetManager.Models;
using BudgetManager.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RepositoriesTests
{
    public class IncomeRespositoryTests
    {
        private BudgetDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<BudgetDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new BudgetDbContext(options);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnIncome_WhenIdAndUserIdAreValidAsync()
        {
            // arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();

            var income = new Income { Id = 1, UserId = userId, Name = "Test Income", Amount = 100, Date = DateTime.Now };
            dbContext.Incomes.Add(income);
            dbContext.SaveChangesAsync();

            var incomeRepository = new IncomeRepository(dbContext);

            // act
            var result = await incomeRepository.GetAsync(1, userId);

            // assert
            result.Should().Be(income);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenIncomeDoesNotExists()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var income = new Income { Id = 1, UserId = userId, Name = "Test Income", Amount = 100, Date = DateTime.Now };
            dbContext.Incomes.Add(income);
            dbContext.SaveChangesAsync();

            var incomeRepository = new IncomeRepository(dbContext);
            //act
            var result = await incomeRepository.GetAsync(3, Guid.NewGuid());

            // assert
            result.Should().Be(null);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenIdIsValidAndUserIdIsInValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var income = new Income { Id = 1, UserId = userId, Name = "Test Income", Amount = 100, Date = DateTime.Now };
            dbContext.Incomes.Add(income);
            dbContext.SaveChangesAsync();

            var incomeRepository = new IncomeRepository(dbContext);
            //act
            var result = await incomeRepository.GetAsync(1, Guid.NewGuid());

            // assert
            result.Should().Be(null);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenIdIsInValidAndUserIdIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var income = new Income { Id = 1, UserId = userId, Name = "Test Income", Amount = 100, Date = DateTime.Now };
            dbContext.Incomes.Add(income);
            dbContext.SaveChangesAsync();

            var incomeRepository = new IncomeRepository(dbContext);
            //act
            var result = await incomeRepository.GetAsync(2, userId);

            // assert
            result.Should().Be(null);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllIncomes_WhenUserIdIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();

            var incomes = new List<Income>
            {
                new Income { Id = 1, UserId = userId, Name = "Income1", Amount = 100, Date = DateTime.Now },
                new Income { Id = 2, UserId = userId, Name = "Income2", Amount = 200, Date = DateTime.Now.AddDays(-1) },
                new Income { Id = 3, UserId = Guid.NewGuid(), Name = "Income3", Amount = 200, Date = DateTime.Now.AddDays(+1) }
            };
            dbContext.AddRange(incomes);
            await dbContext.SaveChangesAsync();

            var incomeRepository = new IncomeRepository(dbContext);

            //act
            var result = await incomeRepository.GetAllAsync(userId);

            //assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenIncomesWithUserIdNotFound()
        {
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();

            var incomes = new List<Income>()
            {
                new Income { Id = 1, UserId = userId, Name = "Income1", Amount = 100, Date = DateTime.Now },
                new Income { Id = 2, UserId = userId, Name = "Income2", Amount = 200, Date = DateTime.Now.AddDays(-1) },
                new Income { Id = 3, UserId = userId, Name = "Income3", Amount = 200, Date = DateTime.Now.AddDays(+1) }

            };
            dbContext.AddRange(incomes);
            await dbContext.SaveChangesAsync();

            var incomeRepository = new IncomeRepository(dbContext);

            //act
            var result = await incomeRepository.GetAllAsync(Guid.NewGuid());

            //assert
            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListSortedByDescendingByDate_WhenUserIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();

            var incomes = new List<Income>()
            {
                new Income { Id = 1, UserId = userId, Name = "Income1", Amount = 100, Date = DateTime.Now },
                new Income { Id = 2, UserId = userId, Name = "Income2", Amount = 200, Date = DateTime.Now.AddDays(-1) },
                new Income { Id = 3, UserId = userId, Name = "Income3", Amount = 200, Date = DateTime.Now.AddDays(+1) }

            };
            dbContext.AddRange(incomes);
            await dbContext.SaveChangesAsync();

            var incomeRepository = new IncomeRepository(dbContext);

            //act
            var result = await incomeRepository.GetAllAsync(userId);

            //assert
            result.Should().BeInDescendingOrder(x => x.Date);
        }
        
        [Fact]
        public async Task AddAsync_ShouldAddNewIncome_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var newIncome = new Income { Id = 1, UserId = Guid.NewGuid(), Name="Test12", Amount = 100, Date = DateTime.Now };
            var incomeRepository = new IncomeRepository(dbContext);
            
            //act
            await incomeRepository.AddAsync(newIncome);

            //assert
            var addedIncome = await dbContext.Incomes.FindAsync(newIncome.Id);
            addedIncome.Should().NotBeNull();
            addedIncome.Should().BeEquivalentTo(newIncome, options => options.Excluding(x => x.Id));
        }
        [Fact]
        public async Task AddAsync_ShouldReturnNewIncome_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var newIncome = new Income { Id = 1, UserId = Guid.NewGuid(), Name = "Test12", Amount = 100, Date = DateTime.Now };
            var incomeRepository = new IncomeRepository(dbContext);

            //act
            var result = await incomeRepository.AddAsync(newIncome);

            //assert
            result.Should().Be(newIncome);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateIncome_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var newIncome = new Income { Id = 1, UserId = Guid.NewGuid(), Name = "Test12", Amount = 100, Date = DateTime.Now };
            var incomeRepository = new IncomeRepository(dbContext);

            //act
            var result = await incomeRepository.AddAsync(newIncome);

            //assert

        }
    }
}