using BudgetManager.Data;
using BudgetManager.Dto.Transaction;
using BudgetManager.Exceptions;
using BudgetManager.Models;
using BudgetManager.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace RepositoriesTests
{
    public class TransactionRepositoryTests
    {
        private BudgetDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<BudgetDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new BudgetDbContext(options);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnTransaction_WhenIdAndUserIdAreValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newTransaction = new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 100, UserId = userId };
            await dbContext.AddAsync(newTransaction);
            await dbContext.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(dbContext);

            //act
            var result = await transactionRepository.GetAsync(1, userId);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Transaction>();
            result.Id.Should().Be(1);
            result.Category.Should().Be(TransactionCategoryEnum.Saves);
            result.Description.Should().Be("desc");
            result.Name.Should().Be("Name");
            result.Price.Should().Be(100);
            result.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenTransactionDoesntExists()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var transactionRepository = new TransactionRepository(dbContext);

            //act
            var result = await transactionRepository.GetAsync(2, userId);

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenIdIsValidAndUserIdIsInValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newTransaction = new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 100, UserId = userId };
            await dbContext.AddAsync(newTransaction);
            await dbContext.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(dbContext);

            //act
            var result = await transactionRepository.GetAsync(1, Guid.Empty);

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenIdIsInValidAndUserIdIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newTransaction = new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 100, UserId = userId };
            await dbContext.AddAsync(newTransaction);
            await dbContext.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(dbContext);

            //act
            var result = await transactionRepository.GetAsync(2, userId);

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnTransactionsList_WhenUserIdIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newTransactionList = new List<Transaction> 
            { 
                new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 100, UserId = userId },
                new Transaction { Id = 2, Category = TransactionCategoryEnum.Entertainment, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 100, UserId = userId }
            };
            await dbContext.AddRangeAsync(newTransactionList);
            await dbContext.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(dbContext);

            //act
            var result = await transactionRepository.GetAllAsync(userId);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Transaction>>();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTransactions_WhenUserIdIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newTransactionList = new List<Transaction>
            {
                new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 100, UserId = userId },
                new Transaction { Id = 2, Category = TransactionCategoryEnum.Entertainment, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 100, UserId = userId },
                new Transaction { Id = 3, Category = TransactionCategoryEnum.Entertainment, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 100, UserId = Guid.Empty }
            };
            await dbContext.AddRangeAsync(newTransactionList);
            await dbContext.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(dbContext);

            //act
            var result = await transactionRepository.GetAllAsync(userId);

            //assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }
        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenTransactionsWithUserIdNotFound()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newTransactionList = new List<Transaction>
            {
                new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 100, UserId = userId },
                new Transaction { Id = 2, Category = TransactionCategoryEnum.Entertainment, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 100, UserId = userId }
            };
            await dbContext.AddRangeAsync(newTransactionList);
            await dbContext.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(dbContext);

            //act
            var result = await transactionRepository.GetAllAsync(Guid.Empty);

            //assert
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewTransaction_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var newTransaction = new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 100, UserId = Guid.NewGuid() };
            var transactionRepository = new TransactionRepository(dbContext);

            //act
            await transactionRepository.AddAsync(newTransaction);

            //assert
            dbContext.Transactions.Should().HaveCount(1);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnNewTransaction_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var preciseDate = DateTime.Now;
            var newTransaction = new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = preciseDate, Description = "desc", Name = "Name", Price = 100, UserId = Guid.NewGuid() };
            var transactionRepository = new TransactionRepository(dbContext);

            //act
            var result = await transactionRepository.AddAsync(newTransaction);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Transaction>();
            result.Id.Should().Be(1);
            result.Category.Should().Be(TransactionCategoryEnum.Saves);
            result.Date.Should().Be(preciseDate);
            result.Description.Should().Be("desc");
            result.Name.Should().Be("Name");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTransaction_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newTransaction = new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 99, UserId = userId };
            await dbContext.AddAsync(newTransaction);
            await dbContext.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(dbContext);

            newTransaction.Date = new DateTime(2024, 8, 8);
            newTransaction.Name = "Name2";
            newTransaction.Description = "desc2";
            newTransaction.Price = 100;

            //act
            await transactionRepository.UpdateAsync(newTransaction);

            //assert
            var transaction = dbContext.Transactions.Find(1);
            transaction.Date.Should().Be(new DateTime(2024, 8, 8));
            transaction.Name.Should().Be("Name2");
            transaction.Description.Should().Be("desc2");
            transaction.Price.Should().Be(100);
        }

        [Fact]
        public async Task UpdateAsync_ShouldNotUpdateTransaction_WhenIdIsInValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var userId = Guid.NewGuid();
            var newTransaction = new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 99, UserId = userId };
            await dbContext.AddAsync(newTransaction);
            await dbContext.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(dbContext);

            newTransaction.Id = 2;

            //act & assert
            await transactionRepository.
                Invoking(async x => await x.UpdateAsync(newTransaction))
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveTransaction_WhenIdIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var newTransaction = new Transaction { Id = 11, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 99, UserId = Guid.NewGuid() };
            await dbContext.AddAsync(newTransaction);
            await dbContext.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(dbContext);

            //act 
            await transactionRepository.DeleteAsync(11);

            //assert
            dbContext.Transactions.Should().HaveCount(0);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotRemoveTransaction_WhenIdIsInValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var newTransaction = new Transaction { Id = 11, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 99, UserId = Guid.NewGuid() };
            await dbContext.AddAsync(newTransaction);
            await dbContext.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(dbContext);

            //act & assert
            await transactionRepository
                .Invoking(async x => await x.DeleteAsync(1))
                .Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldUpdateCategory_WhenDataIsValid()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var newTransaction = new Transaction { Id = 11, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 99, UserId = Guid.NewGuid() };
            await dbContext.AddAsync(newTransaction);
            await dbContext.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(dbContext);

            var updateObject = new UpdateTransactionCategoryDto { Id = newTransaction.Id, Category = TransactionCategoryEnum.Entertainment, UserId = newTransaction.UserId };

            //act 
            await transactionRepository.UpdateCategoryAsync(updateObject);

            //assert
            var result = dbContext.Transactions.Find(11);
            result.Should().NotBeNull();
            result.Category.Should().Be(TransactionCategoryEnum.Entertainment);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldNotUpdateCategory_WhenTransactionNotFound()
        {
            //arrange
            var dbContext = CreateInMemoryDbContext();
            var newTransaction = new Transaction { Id = 11, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "desc", Name = "Name", Price = 99, UserId = Guid.NewGuid() };
            await dbContext.AddAsync(newTransaction);
            await dbContext.SaveChangesAsync();
            var transactionRepository = new TransactionRepository(dbContext);

            var updateObject = new UpdateTransactionCategoryDto { Id = 72, Category = TransactionCategoryEnum.Entertainment, UserId = newTransaction.UserId };

            //act 
            await transactionRepository.
                Invoking(async x => await x.UpdateCategoryAsync(updateObject))
                .Should()
                .ThrowAsync<NullReferenceException>();
        }
    }
}