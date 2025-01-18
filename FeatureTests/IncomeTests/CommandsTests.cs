using BudgetManager.Features.Incomes.Commands;
using BudgetManager.Services;
using FluentAssertions;
using Moq;

namespace FeatureTests.IncomeTests
{
    public class CommandsTests
    {
        private readonly Mock<IIncomeService> _incomeServiceMock;
        private readonly DeleteIncomeCommandHandler _deleteCommandHandler;
        public CommandsTests()
        {
            _incomeServiceMock = new Mock<IIncomeService>();
            _deleteCommandHandler = new DeleteIncomeCommandHandler(_incomeServiceMock.Object);
        }

        [Fact]
        public async Task DeleteIncomeCommandHandler_ShouldCallDeleteIncomeAsyncOnce_WhenDataIsValid()
        {
            //arrange 
            var id = 1;
            var userId = Guid.NewGuid();
            var command = new DeleteIncomeCommand(id, userId);

            //act
            await _deleteCommandHandler.Handle(command, CancellationToken.None);

            //assert
            _incomeServiceMock.Verify(service => service.DeleteIncomeAsync(command.Id, command.UserId), Times.Once);
        }

        [Fact]
        public async Task DeleteIncomeCommandHandler_ShouldThrowArgumentNullException_WhenCommandIsNull()
        {
            //arrange 
            var id = 1;
            var userId = Guid.NewGuid();
            var command = new DeleteIncomeCommand(id, userId);
            command = null;

            //act & assert
            await _deleteCommandHandler
                .Invoking(commnd => commnd.Handle(command, CancellationToken.None))
                .Should()
                .ThrowAsync<ArgumentNullException>(nameof(command));
        }

        [Fact]
        public async Task SaveIncomeCommandHandler_ShouldCallAddIncomeAsyncOnce_WhenDataIsValid()
        {

        }    
        
        [Fact]
        public async Task SaveIncomeCommandHandler_ShouldCallIncomeMapperOnce_WhenDataIsValid()
        {

        }        

        [Fact]
        public async Task SaveIncomeCommandHandler_ShouldThrowArgumentNullException_WhenCommandIsNull()
        {

        }
    }
}