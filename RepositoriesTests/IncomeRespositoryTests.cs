using BudgetManager.Data;
using BudgetManager.Dto;
using BudgetManager.Models;
using BudgetManager.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

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
            await dbContext.SaveChangesAsync();

            var incomeRepository = new IncomeRepository(dbContext);

            // act
            var result = await incomeRepository.GetAsync(1, userId);

            // assert
            result.Should().BeOfType<Income>();
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
            await dbContext.SaveChangesAsync();

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
            await dbContext.SaveChangesAsync();

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
            await dbContext.SaveChangesAsync();

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
            var newIncome = new Income { Id = 1, UserId = Guid.NewGuid(), Name = "Test12", Amount = 100, Date = DateTime.Now };
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
            await dbContext.AddAsync(newIncome);
            await dbContext.SaveChangesAsync();

            newIncome.Amount = 120;
            newIncome.Name = "3333";

            //act
            await incomeRepository.UpdateAsync(newIncome);

            //assert
            var income = await dbContext.Incomes.FindAsync(newIncome.Id);
            income.Should().NotBeNull();
            income.Name.Should().Be("3333");
            income.Amount.Should().Be(120);

        }

        [Fact]
        public async Task UpdateAsync_ShouldNotUpdateIncome_WhenIdIsInValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var newIncome = new Income { Id = 1, UserId = Guid.NewGuid(), Name = "Test12", Amount = 100, Date = DateTime.Now };
            var invalidIncome = new Income { Id = 2, UserId = Guid.NewGuid(), Name = "Inv@lid", Amount = 120, Date = DateTime.Now };
            var incomeRepository = new IncomeRepository(dbContext);
            await dbContext.AddAsync(newIncome);
            await dbContext.SaveChangesAsync();

            //act & assert
            await incomeRepository
                .Invoking(async repo => await repo.UpdateAsync(invalidIncome))
                .Should()
                .ThrowAsync<DbUpdateConcurrencyException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveIncome_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var income = new Income { Id = 1, UserId = Guid.NewGuid(), Name = "Test Income", Amount = 100, Date = DateTime.Now };
            dbContext.Incomes.Add(income);
            await dbContext.SaveChangesAsync();
            var incomeRepository = new IncomeRepository(dbContext);

            //act
            await incomeRepository.DeleteAsync(income);

            //assert
            var deletedIncome = await dbContext.Incomes.FindAsync(income.Id);
            deletedIncome.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotRemoveIncome_WhenIncomeNotFound()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var income = new Income { Id = 1, UserId = Guid.NewGuid(), Name = "Test Income", Amount = 100, Date = DateTime.Now };
            var incomeRepository = new IncomeRepository(dbContext);

            //act & assert

            await incomeRepository
                .Invoking(async x => await x.DeleteAsync(income))
                .Should()
                .ThrowAsync<DbUpdateConcurrencyException>();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnAllIncomes_WhenUserIdAndModelAreValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();

            var incomes = new List<Income>
            {
                new Income { Id = 1, UserId = userId, Name = "Income1", Amount = 100, Date = new DateTime(2024, 5, 23)},
                new Income { Id = 2, UserId = userId, Name = "Income2", Amount = 200, Date = new DateTime(2024, 8, 24) },
                new Income { Id = 3, UserId = Guid.NewGuid(), Name = "Income3", Amount = 200, Date = new DateTime(2024, 8, 21) },
                new Income { Id = 4, UserId = userId, Name = "Income3", Amount = 200, Date = new DateTime(2024, 8, 13) }
            };
            dbContext.AddRange(incomes);
            await dbContext.SaveChangesAsync();
            var monthYearModel = new MonthYearModel { Month = 8, Year = 2024 };
            var incomeRepository = new IncomeRepository(dbContext);

            //act
            var result = await incomeRepository.GetAsync(monthYearModel, userId);

            //assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnEmptyList_WhenIncomesNotFound()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();

            var incomes = new List<Income>();
            dbContext.AddRange(incomes);
            await dbContext.SaveChangesAsync();
            var monthYearModel = new MonthYearModel { Month = 8, Year = 2024 };

            var incomeRepository = new IncomeRepository(dbContext);

            //act
            var result = await incomeRepository.GetAsync(monthYearModel, userId);

            //assert
            result.Should().HaveCount(0);
        }
    }
}