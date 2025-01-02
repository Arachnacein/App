using BudgetManager.Dto.Income;
using BudgetManager.Dto.Transaction;
using BudgetManager.Exceptions;
using BudgetManager.Exceptions.TransactionExceptions;
using BudgetManager.Mappers;
using BudgetManager.Models;
using BudgetManager.Repositories;
using BudgetManager.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace ServicesTests
{
    public class TransactionServiceStests
    {
        private readonly Mock<ITransactionRespository> _transactionRepositoryMock;
        private readonly Mock<ITransactionMapper> _transactionMapperMock;
        private readonly TransactionService _transactionService;
        public TransactionServiceStests()
        {
            _transactionMapperMock = new Mock<ITransactionMapper>();
            _transactionRepositoryMock = new Mock<ITransactionRespository>();
            _transactionService = new TransactionService(_transactionRepositoryMock.Object, _transactionMapperMock.Object);
        }

        [Fact]
        public async Task RetrieveTransactionsAsync_ShouldReturnTransactionDtoList_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var transactionList = new List<Transaction>
            {                
                new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "Description", Name = "Name", Price = 100, UserId = userId },
                new Transaction { Id = 2, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "Description1", Name = "Name1", Price = 100, UserId = userId }
            };
            var transactionListDto = new List<TransactionDto>
            {                
                new TransactionDto { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "Description", Name = "Name", Price = 100, UserId = userId },
                new TransactionDto { Id = 2, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "Description1", Name = "Name1", Price = 100, UserId = userId }
            };
            _transactionRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                .ReturnsAsync(transactionList);
            _transactionMapperMock.Setup(mapper => mapper.MapElements(transactionList))
                .Returns(transactionListDto);

            //act 
            var result = await _transactionService.RetrieveTransactionsAsync(userId);

            //assert
            result.Should().BeOfType<List<TransactionDto>>();
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }     

        [Fact]
        public async Task RetrieveTransactionsAsync_ShouldCallRepositoryOnceAndCallMapperOnce()
        {
            //arrange
            var userId = Guid.NewGuid();
            var transactionList = new List<Transaction>
            {
            };
            var transactionListDto = new List<TransactionDto>
            {
            };
            _transactionRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                .ReturnsAsync(transactionList);
            _transactionMapperMock.Setup(mapper => mapper.MapElements(transactionList))
                .Returns(transactionListDto);

            //act 
            var result = await _transactionService.RetrieveTransactionsAsync(userId);

            //assert
            _transactionMapperMock.Verify(mapper => mapper.MapElements(transactionList), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.GetAllAsync(userId), Times.Once);
        }

        [Fact]
        public async Task RetrieveTransactionAsync_ShouldReturnTransactionDto_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var transaction = new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "Description1", Name = "Name1", Price = 100, UserId = userId };
            var transactionDto = new TransactionDto { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "Description1", Name = "Name1", Price = 100, UserId = userId };
            _transactionRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                .ReturnsAsync(transaction);
            _transactionMapperMock.Setup(mapper => mapper.Map(transaction))
                .Returns(transactionDto);

            //act 
            var result = await _transactionService.RetrieveTransactionAsync(1, userId);

            //assert
            result.Should().BeOfType<TransactionDto>();
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Name1");
        }
        
        [Fact]
        public async Task RetrieveTransactionAsync_ShouldThrowTransactionNotFoundException_WhenIdIsInValidAndUserIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            int wrongId = 2;
            var transaction = new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "Description1", Name = "Name1", Price = 100, UserId = userId };
            var transactionDto = new TransactionDto { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "Description1", Name = "Name1", Price = 100, UserId = userId };
            _transactionRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                .ReturnsAsync(transaction);
            _transactionMapperMock.Setup(mapper => mapper.Map(transaction))
                .Returns(transactionDto);

            //act & Assert
            await _transactionService.
                Invoking(async service => await service.RetrieveTransactionAsync(wrongId, userId))
                .Should()
                .ThrowAsync<TransactionNotFoundException>()
                .WithMessage($"Transaction not found. Id:{wrongId}.");
        }        
        
        [Fact]
        public async Task RetrieveTransactionAsync_ShouldThrowTransactionNotFoundException_WhenIdIsValidAndUserIdIsInValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            Guid wrongUserId = Guid.NewGuid();
            var transaction = new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "Description1", Name = "Name1", Price = 100, UserId = userId };
            var transactionDto = new TransactionDto { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "Description1", Name = "Name1", Price = 100, UserId = userId };
            _transactionRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                .ReturnsAsync(transaction);
            _transactionMapperMock.Setup(mapper => mapper.Map(transaction))
                .Returns(transactionDto);

            //act & Assert
            await _transactionService.
                Invoking(async service => await service.RetrieveTransactionAsync(1, wrongUserId))
                .Should()
                .ThrowAsync<TransactionNotFoundException>()
                .WithMessage($"Transaction not found. Id:{1}.");
        }

        [Fact]
        public async Task RetrieveTransactionAsync_ShouldCallRepositoryOnceAndCallMapperOnce()
        {
            //arrange
            var userId = Guid.NewGuid();
            var transaction = new Transaction { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "Description1", Name = "Name1", Price = 100, UserId = userId };
            var transactionDto = new TransactionDto { Id = 1, Category = TransactionCategoryEnum.Saves, Date = DateTime.Now, Description = "Description1", Name = "Name1", Price = 100, UserId = userId };
            _transactionRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                .ReturnsAsync(transaction);
            _transactionMapperMock.Setup(mapper => mapper.Map(transaction))
                .Returns(transactionDto);

            //act 
            var result = await _transactionService.RetrieveTransactionAsync(1, userId);

            //assert
            _transactionMapperMock.Verify(mapper => mapper.Map(transaction), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.GetAsync(1, userId), Times.Once);
        }

        [Fact]
        public async Task AddTransactionAsync_ShouldThrowNullPointerException_WhenTransactionIsNull()
        {
            //arrange
            var userId = Guid.NewGuid();
            var addTransaction = new AddTransactionDto
            {
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };
            addTransaction = null;

            //act & assert
            await _transactionService.
                    Invoking(async service => await service.AddTransactionAsync(addTransaction))
                    .Should()
                    .ThrowAsync<NullPointerException>()
                    .WithMessage($"Object is null");
        }

        [Theory]
        [InlineData("123")]
        [InlineData("31 characters long sentence. xx")]
        public async Task AddTransactionAsync_ShouldThrowBadStringLengthException_WhenNameLenghtIsInValid(string name)
        {
            //arrange
            var userId = Guid.NewGuid();
            var addTransaction = new AddTransactionDto
            {
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = name,
                Price = 100,
                UserId = userId
            };

            //act & assert
            await _transactionService.
                    Invoking(async service => await service.AddTransactionAsync(addTransaction))
                    .Should()
                    .ThrowAsync<BadStringLengthException>()
                    .WithMessage("Name have incorrect length. Should be more than 3 and less than 30.");
        }

        [Theory]
        [InlineData("123")]
        [InlineData("151 characteres long sentence.daddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd_end")]
        public async Task AddTransactionAsync_ShouldThrowBadStringLengthException_WhenDescriptionIsNotNullAndDescriptionLengthIsInValid(string description)
        {
            //arrange
            var userId = Guid.NewGuid();
            var addTransaction = new AddTransactionDto
            {
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = description,
                Name = "Name",
                Price = 100,
                UserId = userId
            };

            //act & assert
            await _transactionService.
                    Invoking(async service => await service.AddTransactionAsync(addTransaction))
                    .Should()
                    .ThrowAsync<BadStringLengthException>()
                    .WithMessage("Description have incorrect length. Should be more than 3 and less than 150.");
        }

        [Theory]
        [InlineData(-0.0001)]
        [InlineData(-1)]
        public async Task AddTransactionAsync_ShouldThrowBadValueException_WhenPriceIsInValid(double price)
        {
            //arrange
            var userId = Guid.NewGuid();
            var addTransaction = new AddTransactionDto
            {
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = null,
                Name = "Name",
                Price = price,
                UserId = userId
            };

            //act & assert
            await _transactionService.
                    Invoking(async service => await service.AddTransactionAsync(addTransaction))
                    .Should()
                    .ThrowAsync<BadValueException>()
                    .WithMessage($"Price is incorrect. {price}");
        }

        [Fact]
        public async Task AddTransactionAsync_ShouldAddNewTransaction_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var addTransaction = new AddTransactionDto
            {
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };            
            var transaction = new Transaction
            {
                Id = 1,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };           
            var transactionDto = new TransactionDto
            {
                Id = transaction.Id,
                Category = transaction.Category,
                Date = transaction.Date,
                Description = transaction.Description,
                Name = transaction.Name,
                Price = transaction.Price,
                UserId = transaction.UserId
            };

            _transactionMapperMock.Setup(mapper => mapper.Map(addTransaction))
                .Returns(transaction);
            _transactionRepositoryMock.Setup(repo => repo.AddAsync(transaction));
            _transactionMapperMock.Setup(mapper => mapper.Map(transaction))
                .Returns(transactionDto);

            await _transactionService.AddTransactionAsync(addTransaction);

            //assert
            var addedTransaction = _transactionService.RetrieveTransactionAsync(1, userId);
            addedTransaction.Should().NotBeNull();
        }

        [Fact]
        public async Task AddTransactionAsync_ShouldReturnTransactionDto_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var addTransaction = new AddTransactionDto
            {
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };
            var transaction = new Transaction
            {
                Id = 1,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };
            var transactionDto = new TransactionDto
            {
                Id = transaction.Id,
                Category = transaction.Category,
                Date = transaction.Date,
                Description = transaction.Description,
                Name = transaction.Name,
                Price = transaction.Price,
                UserId = transaction.UserId
            };

            _transactionMapperMock.Setup(mapper => mapper.Map(addTransaction))
                .Returns(transaction);
            _transactionRepositoryMock.Setup(repo => repo.AddAsync(transaction));
            _transactionMapperMock.Setup(mapper => mapper.Map(transaction))
                .Returns(transactionDto);

            var result = await _transactionService.AddTransactionAsync(addTransaction);

            //assert
            result.Should().BeOfType<TransactionDto>();

        }

        [Fact]
        public async Task AddTransactionAsync_ShouldCallRepositoryOnceAndCallMapperTwice()
        {
            //arrange
            var userId = Guid.NewGuid();
            var addTransaction = new AddTransactionDto
            {
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };
            var transaction = new Transaction
            {
                Id = 1,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };

            _transactionMapperMock.Setup(mapper => mapper.Map(addTransaction))
                .Returns(transaction);
            _transactionRepositoryMock.Setup(repo => repo.AddAsync(transaction));

            var result = await _transactionService.AddTransactionAsync(addTransaction);

            //assert
            _transactionRepositoryMock.Verify(repo => repo.AddAsync(transaction), Times.Once);
            _transactionMapperMock.Verify(mapper => mapper.Map(addTransaction), Times.Once);
            _transactionMapperMock.Verify(mapper => mapper.Map(transaction), Times.Once);
        }
        
        [Fact]
        public async Task UpdateTransactionAsync_ShouldThrowNullPointerException_WhenTransactionIsNull()
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateTransactionDto = new UpdateTransactionDto
            {
                Id = 1,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };
            updateTransactionDto = null;

            //act & assert
            await _transactionService
                .Invoking(async service => await service.UpdateTransactionAsync(updateTransactionDto))
                .Should()
                .ThrowAsync<NullPointerException>()
                .WithMessage("Object is null");
        }

        [Theory]
        [InlineData("123")]
        [InlineData("31 characters long sentence.xx")]
        public async Task UpdateTransactionAsync_ShouldThrowBadStringLengthException_WhenNameLenghtIsInValid(string name)
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateTransactionDto = new UpdateTransactionDto
            {
                Id = 1,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = name,
                Price = 100,
                UserId = userId
            };

            //act & assert
            await _transactionService
                .Invoking(async service => await service.UpdateTransactionAsync(updateTransactionDto))
                .Should()
                .ThrowAsync<BadStringLengthException>()
                .WithMessage("Name have incorrect length. Should be more than 3 and less than 30.");
        }

        [Theory]
        [InlineData("123")]
        [InlineData("151 characteres long sentence.daddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd_end")]
        public async Task UpdateTransactionAsync_ShouldThrowBadStringLengthException_WhenDescriptionIsNotNullAndDescriptionLengthIsInValid(string description)
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateTransactionDto = new UpdateTransactionDto
            {
                Id = 1,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = description,
                Name = "Name",
                Price = 100,
                UserId = userId
            };

            //act & assert
            await _transactionService
                .Invoking(async service => await service.UpdateTransactionAsync(updateTransactionDto))
                .Should()
                .ThrowAsync<BadStringLengthException>()
                .WithMessage("Description have incorrect length. Should be more than 3 and less than 150.");
        }

        [Theory]
        [InlineData(-1)]
        public async Task UpdateTransactionAsync_ShouldThrowBadValueException_WhenPriceIsInValid(double price)
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateTransactionDto = new UpdateTransactionDto
            {
                Id = 1,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description",
                Name = "Name",
                Price = price,
                UserId = userId
            };

            //act & assert
            await _transactionService
                .Invoking(async service => await service.UpdateTransactionAsync(updateTransactionDto))
                .Should()
                .ThrowAsync<BadValueException>()
                .WithMessage($"Price is incorrect. {price}");
        }

        [Fact]
        public async Task UpdateTransactionAsync_ShouldUpdateTransaction_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateTransactionDto = new UpdateTransactionDto
            {
                Id = 1,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };            
            var transaction = new Transaction
            {
                Id = 1,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };
            Transaction caught = null;

            _transactionMapperMock
                .Setup(mapper => mapper.Map(updateTransactionDto))
                .Returns((UpdateTransactionDto dto) => new Transaction
                {
                    Id = dto.Id,
                    UserId = dto.UserId,
                    Price = dto.Price,
                    Date = dto.Date,
                    Name = dto.Name,
                    Description = dto.Description,
                    Category = dto.Category
                });

            _transactionRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Transaction>()))
                .Callback<Transaction>(transaction =>
                {
                    caught = transaction;
                })
                .Returns(Task.CompletedTask);

            updateTransactionDto.Name = "Name2";
            updateTransactionDto.Price = 2500;

            //act
            await _transactionService.UpdateTransactionAsync(updateTransactionDto);

            //assert
            caught.Name.Should().Be("Name2");
            caught.Price.Should().Be(2500);
            caught.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task UpdateTransactionAsync_ShouldCallRepositoryOnceAndCallMapperOnce()
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateTransactionDto = new UpdateTransactionDto
            {
                Id = 1,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };
            var transaction = new Transaction
            {
                Id = 1,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };

            _transactionMapperMock
                .Setup(mapper => mapper.Map(updateTransactionDto))
                .Returns(transaction);

            _transactionRepositoryMock
                .Setup(repo => repo.UpdateAsync(transaction))
                .Returns(Task.CompletedTask);

            //act
            await _transactionService.UpdateTransactionAsync(updateTransactionDto);

            //assert
            _transactionMapperMock.Verify(mapper => mapper.Map(updateTransactionDto), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.UpdateAsync(transaction), Times.Once);
        }

        [Fact]
        public async Task DeleteTransactionAsync_ShouldCallRepositoryTwice()
        {
            //arrange
            var userId = Guid.NewGuid();
            var transaction = new Transaction
            {
                Id = 1,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };
            _transactionRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                                 .ReturnsAsync(transaction);

            _transactionRepositoryMock.Setup(repo => repo.DeleteAsync(1))
                                 .Returns(Task.CompletedTask);

            //act
            await _transactionService.DeleteTransactionAsync(1, userId);

            //assert
            _transactionRepositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.GetAsync(1, userId), Times.Once);
        }

        [Fact]
        public async Task DeleteTransactionAsync_ShouldRemoveTransaction_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var transaction = new Transaction
            {
                Id = 1,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };
            _transactionRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                .ReturnsAsync(transaction);

            _transactionRepositoryMock.Setup(repo => repo.DeleteAsync(1))
                .Callback(() =>
                    _transactionRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                        .ReturnsAsync((Transaction)null)
                )
                .Returns(Task.CompletedTask);

            //act 
            await _transactionService.RetrieveTransactionAsync(1, userId);

            //assert
            var result = await _transactionService.RetrieveTransactionAsync(1, userId);
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteTransactionAsync_ShouldThrowNullPointerException_WhenTransactionIsNull()
        {
            //arrange
            var userId = Guid.NewGuid();
            var wrongId = 2;
            var transaction = new Transaction
            {
                Id = 1,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };
            _transactionRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                                 .ReturnsAsync(transaction);

            _transactionRepositoryMock.Setup(repo => repo.DeleteAsync(1))
                                 .Returns(Task.CompletedTask);

            //act & assert
            await _transactionService
                .Invoking(async service => await service.DeleteTransactionAsync(wrongId, userId))
                .Should()
                .ThrowAsync<NullPointerException>()
                .WithMessage($"Transaction not found. Id:{wrongId}.");
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldCallRepositoryTwice()
        {
            //arrange
            var userId = Guid.NewGuid();
            var transactionId = 1;
            var transaction = new Transaction
            {
                Id = transactionId, 
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description",
                Name = "Name",
                Price = 100,
                UserId = userId
            };
            var updateCategory = new UpdateTransactionCategoryDto
            {
                Id = transactionId,
                UserId = userId,
                Category = TransactionCategoryEnum.Saves
            };
            _transactionRepositoryMock.Setup(repo => repo.GetAsync(transactionId, userId))
                .ReturnsAsync(transaction);
            _transactionRepositoryMock.Setup(repo => repo.UpdateCategoryAsync(updateCategory));

            //act
            await _transactionService.UpdateCategoryAsync(updateCategory);

            //assert
            _transactionRepositoryMock.Verify(repo => repo.GetAsync(transactionId, userId), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.UpdateCategoryAsync(updateCategory), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldThrowTransactionNotFoundException_WhenTransactionIsNull()
        {
            //arrange
            var userId = Guid.NewGuid();
            var transactionId = 1;
            var transaction = new Transaction
            {
                Id = transactionId,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description",
                Name = "Name",
                Price = 100,
                UserId = userId
            };
            var updateCategory = new UpdateTransactionCategoryDto
            {
                Id = transactionId,
                UserId = userId,
                Category = TransactionCategoryEnum.Saves
            };
            _transactionRepositoryMock.Setup(repo => repo.GetAsync(2, userId))
                .ReturnsAsync(transaction);
            _transactionRepositoryMock.Setup(repo => repo.UpdateCategoryAsync(updateCategory));

            //act & assert
            await _transactionService
                .Invoking(async service => await service.UpdateCategoryAsync(updateCategory))
                .Should()
                .ThrowAsync<TransactionNotFoundException>();
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldThrowBadValueException_WhenCategoryNotFound()
        {
            //arrange
            var userId = Guid.NewGuid();
            var transactionId = 1;
            var transaction = new Transaction
            {
                Id = transactionId,
                Category = TransactionCategoryEnum.Saves,
                Date = DateTime.Now,
                Description = "Description",
                Name = "Name",
                Price = 100,
                UserId = userId
            };
            var updateCategory = new UpdateTransactionCategoryDto
            {
                Id = transactionId,
                UserId = userId,
                Category = (TransactionCategoryEnum)77
            };
            _transactionRepositoryMock.Setup(repo => repo.GetAsync(transactionId, userId))
                .ReturnsAsync(transaction);
            _transactionRepositoryMock.Setup(repo => repo.UpdateCategoryAsync(updateCategory));

            //act & assert
            await _transactionService
                .Invoking(async service => await service.UpdateCategoryAsync(updateCategory))
                .Should()
                .ThrowAsync<BadValueException>();
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldUpdateCategory()
        {
            //arrange
            var userId = Guid.NewGuid();
            var transactionId = 1;
            var oldCategory = TransactionCategoryEnum.Saves;
            var newCategory = TransactionCategoryEnum.Entertainment;
            var transaction = new Transaction
            {
                Id = transactionId,
                Category = oldCategory,
                Date = DateTime.Now,
                Description = "Description1",
                Name = "Name1",
                Price = 100,
                UserId = userId
            };
            var updateCategory = new UpdateTransactionCategoryDto
            {
                Id = transactionId,
                UserId = userId,
                Category = newCategory
            };
            _transactionRepositoryMock.Setup(repo => repo.GetAsync(transactionId, userId))
                .ReturnsAsync(transaction);
            _transactionRepositoryMock.Setup(repo => repo.UpdateCategoryAsync(updateCategory))
                .Callback(() => transaction.Category = newCategory) 
                .Returns(Task.CompletedTask);

            //act
            await _transactionService.UpdateCategoryAsync(updateCategory);

            //assert
            transaction.Category.Should().Be(newCategory);
        }
    }
}