using BudgetManager.Mappers;
using BudgetManager.Repositories;
using BudgetManager.Services;
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

        }

        [Fact]
        public async Task RetrieveTransactionsAsync_ShouldCallRepositoryOnceAndCallMapperOnce()
        {

        }

        [Fact]
        public async Task RetrieveTransactionAsync_ShouldThrowTransactionNotFoundException_WhenIdIsValidAndUserIdIsInValid()
        {

        }
        
        [Fact]
        public async Task RetrieveTransactionAsync_ShouldThrowTransactionNotFoundException_WhenIdIsInValidAndUserIdIsValid()
        {

        }

        [Fact]
        public async Task RetrieveTransactionAsync_ShouldReturnTransactionDto_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task RetrieveTransactionAsync_ShouldCallRepositoryOnceAndCallMapperOnce()
        {

        }

        [Fact]
        public async Task AddTransactionAsync_ShouldThrowNullPointerException_WhenTransactionIsNull()
        {

        }

        [Theory]
        [InlineData("123")]
        [InlineData("31 characteres long sentence")]
        public async Task AddTransactionAsync_ShouldThrowBadStringLengthException_WhenNameLenghtIsInValid(string name)
        {

        }

        [Theory]
        [InlineData("123")]
        [InlineData("151 characteres long sentence")]
        public async Task AddTransactionAsync_ShouldThrowBadStringLengthException_WhenDescriptionIsNotNullAndDescriptionLengthIsInValid(string description)
        {

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task AddTransactionAsync_ShouldThrowBadValueException_WhenPriceIsInValid(double price)
        {

        }

        [Fact]
        public async Task AddTransactionAsync_ShouldAddNewTransaction_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task AddTransactionAsync_ShouldReturnTransactionDto_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task AddTransactionAsync_ShouldCallRepositoryOnceAndCallMapperTwice()
        {

        }
        
        [Fact]
        public async Task UpdateTransactionAsync_ShouldThrowNullPointerException_WhenTransactionIsNull()
        {

        }

        [Theory]
        [InlineData("123")]
        [InlineData("31 characteres long sentence")]
        public async Task UpdateTransactionAsync_ShouldThrowBadStringLengthException_WhenNameLenghtIsInValid(string name)
        {

        }

        [Theory]
        [InlineData("123")]
        [InlineData("151 characteres long sentence")]
        public async Task UpdateTransactionAsync_ShouldThrowBadStringLengthException_WhenDescriptionIsNotNullAndDescriptionLengthIsInValid(string description)
        {

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task UpdateTransactionAsync_ShouldThrowBadValueException_WhenPriceIsInValid(double price)
        {

        }

        [Fact]
        public async Task UpdateTransactionAsync_ShouldUpdateTransaction_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task UpdateTransactionAsync_ShouldCallRepositoryOnceAndCallMapperOnce()
        {

        }       
        
        [Fact]
        public async Task DeleteTransactionAsync_ShouldCallRepositoryTwice()
        {

        }

        [Fact]
        public async Task DeleteTransactionAsync_ShouldRemoveTransaction_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task DeleteTransactionAsync_ShouldThrowNullPointerException_WhenTransactionIsNull()
        {

        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldCallRepositoryTwice()
        {

        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldThrowTransactionNotFoundException_WhenTransactionIsNull()
        {

        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldThrowBadValueException_WhenCategoryNotFound()
        {

        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldUpdateCategory()
        {

        }
    }
}