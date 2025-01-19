using BudgetManager.Dto.Income;
using BudgetManager.Features.Incomes.Commands;
using BudgetManager.Mappers;
using BudgetManager.Services;
using FluentAssertions;
using Moq;

namespace FeatureTests.IncomeTests.Commands
{
    public class SaveIncomeCommandTests
    {
        private readonly Mock<IIncomeService> _incomeServiceMock;
        private readonly Mock<IIncomeMapper> _incomeMapperMock;
        private readonly SaveIncomeCommandHandler _saveIncomeCommandHandlerMock;

        public SaveIncomeCommandTests()
        {
            _incomeMapperMock = new Mock<IIncomeMapper>();
            _incomeServiceMock = new Mock<IIncomeService>();
            _saveIncomeCommandHandlerMock = new SaveIncomeCommandHandler(_incomeServiceMock.Object, _incomeMapperMock.Object);
        }

        [Fact]
        public async Task SaveIncomeCommandHandler_ShouldCallAddIncomeAsyncOnce_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var command = new SaveIncomeCommand(userId, "name", 100, date);
            var addIncomeDto = new AddIncomeDto { UserId = userId, Amount = 100, Date = date, Name = "name"};
            var incomeDto = new IncomeDto { Id = 1, UserId = userId, Amount = 100, Date = date, Name = "name"};

            _incomeMapperMock.Setup(mapper => mapper.Map(command))
                .Returns(addIncomeDto);
            _incomeServiceMock.Setup(service => service.AddIncomeAsync(addIncomeDto))
                .ReturnsAsync(incomeDto);

            //act
            await _saveIncomeCommandHandlerMock.Handle(command, CancellationToken.None);

            //assert
            _incomeServiceMock.Verify(service => service.AddIncomeAsync(addIncomeDto), Times.Once);
        }

        [Fact]
        public async Task SaveIncomeCommandHandler_ShouldCallIncomeMapperOnce_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var command = new SaveIncomeCommand(userId, "name", 100, date);
            var addIncomeDto = new AddIncomeDto { UserId = userId, Amount = 100, Date = date, Name = "name" };
            var incomeDto = new IncomeDto { Id = 1, UserId = userId, Amount = 100, Date = date, Name = "name" };

            _incomeMapperMock.Setup(mapper => mapper.Map(command))
                .Returns(addIncomeDto);
            _incomeServiceMock.Setup(service => service.AddIncomeAsync(addIncomeDto))
                .ReturnsAsync(incomeDto);

            //act
            await _saveIncomeCommandHandlerMock.Handle(command, CancellationToken.None);

            //assert
            _incomeMapperMock.Verify(mapper => mapper.Map(command), Times.Once);

        }

        [Fact]
        public async Task SaveIncomeCommandHandler_ShouldThrowArgumentNullException_WhenCommandIsNull()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var command = new SaveIncomeCommand(userId, "name", 100, date);
            command = null;

            //act & assert
            await _saveIncomeCommandHandlerMock
               .Invoking(commnd => commnd.Handle(command,CancellationToken.None))
               .Should()
               .ThrowAsync<ArgumentNullException>(nameof(command));
        }
    }
}