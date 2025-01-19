using BudgetManager.Dto.Income;
using BudgetManager.Features.Incomes.Commands;
using BudgetManager.Mappers;
using BudgetManager.Services;
using FluentAssertions;
using Moq;

namespace FeatureTests.IncomeTests.CommandsTests
{
    public class UpdateIncomeCommandHandlerTests
    {
        private readonly Mock<IIncomeService> _incomeServiceMock;
        private readonly Mock<IIncomeMapper> _incomeMapperMock;
        private readonly UpdateIncomeCommandHandler _updateCommandHandler;
        public UpdateIncomeCommandHandlerTests()
        {
            _incomeMapperMock = new Mock<IIncomeMapper>();
            _incomeServiceMock = new Mock<IIncomeService>();
            _updateCommandHandler = new UpdateIncomeCommandHandler(_incomeServiceMock.Object, _incomeMapperMock.Object);
        }

        [Fact]
        public async Task UpdateIncomeCommandHandler_ShouldThrowArgumentNulException_WhenCommandIsNull()
        {
            //arrange
            var command = new UpdateIncomeCommand(1, Guid.NewGuid(), "Name", 100, DateTime.Now);
            command = null;

            //act & assert
            await _updateCommandHandler
                .Invoking(commnd => commnd.Handle(command, CancellationToken.None))
                .Should()
                .ThrowAsync<ArgumentNullException>(nameof(command));
        }

        [Fact]
        public async Task UpdateIncomeCommandHandler_ShouldCallUpdateIncomeAsyncOnce_WhenDataIsValid()
        {
            //arrange
            var command = new UpdateIncomeCommand(1, Guid.NewGuid(), "Name", 100, DateTime.Now);
            var updateIncomeDto = new UpdateIncomeDto { Id = 1, UserId = Guid.NewGuid(), Name = "Name", Amount = 100, Date = DateTime.Now };
            _incomeMapperMock.Setup(mapper => mapper.Map(command))
                .Returns(updateIncomeDto);
            _incomeServiceMock.Setup(service => service.UpdateIncomeAsync(updateIncomeDto))
                .Returns(Task.CompletedTask);
            //act


            //assert
        }
    }
}