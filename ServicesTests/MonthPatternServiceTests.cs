using BudgetManager.Mappers;
using BudgetManager.Repositories;
using BudgetManager.Services;
using Moq;

namespace ServicesTests
{
    public class MonthPatternServiceTests
    {
        private readonly Mock<IMonthPatternRepository> _monthPatternRepositoryMock;
        private readonly Mock<IMonthPatternMapper> _monthPatternMapperMock;
        private readonly Mock<IPatternRepository> _patternRepositoryMock;
        private readonly Mock<IPatternMapper> _patternMapperMock;
        private readonly MonthPatternService _monthPatternService;
        public MonthPatternServiceTests()
        {
            _monthPatternRepositoryMock = new Mock<IMonthPatternRepository>();
            _monthPatternMapperMock = new Mock<IMonthPatternMapper>();
            _patternRepositoryMock = new Mock<IPatternRepository>();
            _patternMapperMock = new Mock<IPatternMapper>();
            _monthPatternService = new MonthPatternService(_monthPatternRepositoryMock.Object,
                                                           _monthPatternMapperMock.Object,
                                                           _patternRepositoryMock.Object,
                                                           _patternMapperMock.Object);
        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldReturnMonthPattern_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldReturnMonthPatternDto_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldThrowPatternNotFoundException_WhenIdIsInValidAndUserIdIsValid()
        {

        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldThrowPatternNotFoundException_WhenIdIsValidAndUserIdIsInValid()
        {

        }        
        
        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldCallRepositoryOnceAndMapperOnce_WhenCalled()
        {

        }

        [Fact]
        public async Task RetrieveMonthPatternsAsync_ShouldReturnList_WhenUserIdIsValid()
        {

        }

        [Fact]
        public async Task RetrieveMonthPatternsAsync_ShouldReturnEmptyList_WhenUserIdIsInValid()
        {

        }

        [Fact]
        public async Task RetrieveMonthPatternsAsync_ShouldReturnEmptyList_WhenNotFoundAnyMonthPatterns()
        {

        }

        [Fact]
        public async Task RetrieveMonthPatternsAsync_ShouldCallRepositoryOnceAndCallMapperOnce()
        {

        }

        [Fact]
        public async Task AddMonthPatternAsync_ShouldAddMonthPattern_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task AddMonthPatternAsync_ShouldThrowPatternNotFoundException_WhenPatternDoesntExists()
        {

        }

        [Fact]///???????
        public async Task AddMonthPatternAsync_ShouldThrowMonthPatternAlreadyExistsException_WhenPatternAlreadyExists()
        {

        }

        [Fact]
        public async Task AddMonthPatternAsync_ShouldCallRepositoryThriceAndCallMapperTwice_WhenCalled()
        {

        }

        [Fact]
        public async Task AddMonthPatternAsync_ShouldReturnNewMonthPattern_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task UpdateMonthPatternAsync_ShouldCallRepositoryTwiceAndCallMapperOnce_WhenCalled()
        {

        }

        [Fact]
        public async Task UpdateMonthPatternAsync_ShouldThrowMonthPatternNotFoundException_WhenIdIsValidAndUserIdIsInValid()
        {

        }       
        
        [Fact]
        public async Task UpdateMonthPatternAsync_ShouldThrowMonthPatternNotFoundException_WhenIdIsInValidAndUserIdIsValid()
        {

        }

        [Fact]
        public async Task UpdateMonthPatternAsync_ShouldUpdateMonthPattern_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task DeleteMonthPatternAsync_ShouldThrowMonthPatternNotFoundException_WhenIdIsValidAndUserIdIsInValid()
        {

        }
        
        [Fact]
        public async Task DeleteMonthPatternAsync_ShouldThrowMonthPatternNotFoundException_WhenIdIsInValidAndUserIdIsValid()
        {

        }

        [Fact]
        public async Task DeleteMonthPatternAsync_ShouldDeleteMonthPattern_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task DeleteMonthPatternAsync_ShouldCallRepositoryTwice_WhenCalled()
        {

        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldReturnPatternDtoWithValueMinusOne_WhenMonthPatternNotFound()
        {

        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldThrowPatternNotFoundException_WhenPatternNotFound()
        {

        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldReturnPatternDto_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldCallRepositoryTwiceAndCallMapperOnce()
        {

        }

        [Fact]
        public async Task RetrievePatternsAsync_ShouldReturnFullMonthPatternDtoList_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task RetrievePatternsAsync_ShouldReturnEmptyList_WhenUserIdIsInValid()
        {

        }

        [Fact]
        public async Task RetrievePatternsAsync_ShouldCallRepositoryTwice_WhenCalled()
        {

        }
    }
}