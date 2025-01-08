
using BudgetManager.Dto.Transaction;
using BudgetManager.Features.Transactions.Commands;
using BudgetManager.Mappers;
using BudgetManager.Models;
using FluentAssertions;
using Moq;

namespace MappersTests
{
    public class TransactionMapperTests
    {
        private readonly ITransactionMapper _mapper;
        public TransactionMapperTests()
        {
            _mapper = new TransactionMapper();
        }

        [Fact]
        public async Task  Map_TransactionToTransactionDto_ShouldReturnTransationDto_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var transaction = new Transaction
            {
                Id = 1,
                UserId = userId,
                Name = "Name",
                Description = "Description",
                Date = date,
                Price = 100.0,
                Category = TransactionCategoryEnum.Entertainment
            };

            //act
            var mappedTransaction = _mapper.Map(transaction);

            //assert
            mappedTransaction.Should().NotBeNull();
            mappedTransaction.Should().BeOfType<TransactionDto>();
            mappedTransaction.Should().BeEquivalentTo(transaction);
        }       
        [Fact]
        public async Task  Map_SaveTransactionCommandToAddTransactionDto_ShouldReturnAddTransationDto_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var saveTransactionCommand = new SaveTransactionCommand
                ("Name", "Description", date, 100, TransactionCategoryEnum.Entertainment, userId);

            //act
            var mappedTransaction = _mapper.Map(saveTransactionCommand);

            //assert
            mappedTransaction.Should().NotBeNull();
            mappedTransaction.Should().BeOfType<AddTransactionDto>();
            mappedTransaction.Should().BeEquivalentTo(saveTransactionCommand);
        }       
        [Fact]
        public async Task  Map_UpdateTransactionCommandToUpdateTransactionDto_ShouldReturnUpdateTransationDto_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var updateTransactionCommand = new UpdateTransactionCommand
                (1, userId, "Name", "Description", date, 100, TransactionCategoryEnum.Entertainment);

            //act
            var mappedTransaction = _mapper.Map(updateTransactionCommand);

            //assert
            mappedTransaction.Should().NotBeNull();
            mappedTransaction.Should().BeOfType<UpdateTransactionDto>();
            mappedTransaction.Should().BeEquivalentTo(updateTransactionCommand);
        }       
        [Fact]
        public async Task  Map_TransactionDtoToTransaction_ShouldReturnTransation_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var transactionDto = new TransactionDto
            {
                Id = 1,
                UserId = userId,
                Name = "Name",
                Description = "Description",
                Date = date,
                Price = 100.0,
                Category = TransactionCategoryEnum.Entertainment
            };

            //act
            var mappedTransaction = _mapper.Map(transactionDto);

            //assert
            mappedTransaction.Should().NotBeNull();
            mappedTransaction.Should().BeOfType<Transaction>();
            mappedTransaction.Should().BeEquivalentTo(transactionDto);
        }        
        [Fact]
        public async Task  Map_AddTransactionDtoToTransaction_ShouldReturnAddTransation_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var addTransactionDto = new AddTransactionDto
            {
                UserId = userId,
                Name = "Name",
                Description = "Description",
                Date = date,
                Price = 100.0,
                Category = TransactionCategoryEnum.Entertainment
            };

            //act
            var mappedTransaction = _mapper.Map(addTransactionDto);

            //assert
            mappedTransaction.Should().NotBeNull();
            mappedTransaction.Should().BeOfType<Transaction>();
            mappedTransaction.Should().BeEquivalentTo(addTransactionDto);
        }        
        [Fact]
        public async Task  Map_UpdateTransactionDtoToTransaction_ShouldReturnUpdateTransation_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var updateTransactionDto = new UpdateTransactionDto
            {
                Id = 1,
                UserId = userId,
                Name = "Name",
                Description = "Description",
                Date = date,
                Price = 100.0,
                Category = TransactionCategoryEnum.Entertainment
            };

            //act
            var mappedTransaction = _mapper.Map(updateTransactionDto);

            //assert
            mappedTransaction.Should().NotBeNull();
            mappedTransaction.Should().BeOfType<Transaction>();
            mappedTransaction.Should().BeEquivalentTo(updateTransactionDto);
        }        
        [Fact]
        public async Task  MapElements_TransactionListToTransactionDtoList_ShouldReturnTransationDtoList_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var transactionDtoList = new List<Transaction> 
            { 
                new Transaction { Id = 1, UserId = userId, Name = "Name", Description = "Description", Date = date, Price = 100.0, Category = TransactionCategoryEnum.Entertainment },
                new Transaction { Id = 2, UserId = userId, Name = "Name", Description = "Description", Date = date, Price = 100.0, Category = TransactionCategoryEnum.Entertainment },
                new Transaction { Id = 3, UserId = userId, Name = "Name", Description = "Description", Date = date, Price = 100.0, Category = TransactionCategoryEnum.Entertainment }
            };

            //act
            var mappedTransactionsList = _mapper.MapElements(transactionDtoList);

            //assert
            mappedTransactionsList.Should().NotBeNull();
            mappedTransactionsList.Should().BeOfType<List<TransactionDto>>();
            mappedTransactionsList.Should().BeEquivalentTo(transactionDtoList);
        }        
        [Fact]
        public async Task  MapElements_TransactionDtoListToTransactionList_ShouldReturnTransationList_WhenCalled()
        {            
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var transactionDtoList = new List<TransactionDto>
            {
                new TransactionDto { Id = 1, UserId = userId, Name = "Name", Description = "Description", Date = date, Price = 100.0, Category = TransactionCategoryEnum.Entertainment },
                new TransactionDto { Id = 2, UserId = userId, Name = "Name", Description = "Description", Date = date, Price = 100.0, Category = TransactionCategoryEnum.Entertainment },
                new TransactionDto { Id = 3, UserId = userId, Name = "Name", Description = "Description", Date = date, Price = 100.0, Category = TransactionCategoryEnum.Entertainment }
            };

            //act
            var mappedTransactionsList = _mapper.MapElements(transactionDtoList);

            //assert
            mappedTransactionsList.Should().NotBeNull();
            mappedTransactionsList.Should().BeOfType<List<Transaction>>();
            mappedTransactionsList.Should().BeEquivalentTo(transactionDtoList);
        } 
    }
}