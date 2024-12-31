using BudgetManager.Dto;
using BudgetManager.Dto.Income;
using BudgetManager.Exceptions;
using BudgetManager.Exceptions.IncomeExceptions;
using BudgetManager.Mappers;
using BudgetManager.Models;
using BudgetManager.Repositories;
using BudgetManager.Services;
using FluentAssertions;
using Moq;

namespace ServicesTests
{
    public class IncomeServiceTests
    {
        private readonly Mock<IIncomeRepository> _incomeRepositoryMock;
        private readonly Mock<IIncomeMapper> _incomeMapperMock;
        private readonly IncomeService _incomeService;

        public IncomeServiceTests()
        {
            _incomeRepositoryMock = new Mock<IIncomeRepository>();
            _incomeMapperMock = new Mock<IIncomeMapper>();
            _incomeService = new IncomeService(_incomeRepositoryMock.Object, _incomeMapperMock.Object);
        }

        [Fact]
        public async Task RetrieveIncomesAsync_ShouldReturnMappedIncomes_WhenIdIsValid()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var incomes = new List<Income>
            {
                new Income { Id = 1, UserId = userId, Amount = 100 },
                new Income { Id = 2, UserId = userId, Amount = 200 }
            };
            var mappedIncomes = new List<IncomeDto>
            {
                new IncomeDto { Id = incomes.First().Id, UserId = incomes.First().UserId, Amount = incomes.First().Amount },
                new IncomeDto { Id = incomes[1].Id, UserId = incomes[1].UserId, Amount = incomes[1].Amount }
            };

            _incomeRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                                 .ReturnsAsync(incomes);
            _incomeMapperMock.Setup(mapper => mapper.MapElements(incomes))
                              .Returns(mappedIncomes);

            //act
            var result = await _incomeService.RetrieveIncomesAsync(userId);

            //assert

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(incomes);
        }

        [Fact]
        public async Task RetrieveIncomesAsync_ShouldReturnEmptyList_WhenIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var incomes = new List<Income>
            {
                new Income { Id = 1, UserId = userId, Amount = 100 },
                new Income { Id = 2, UserId = userId, Amount = 200 }
            };
            var mappedIncomes = new List<IncomeDto>
            {
            };

            _incomeRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                                 .ReturnsAsync(incomes);
            _incomeMapperMock.Setup(mapper => mapper.MapElements(incomes))
                              .Returns(mappedIncomes);

            //act
            var result = await _incomeService.RetrieveIncomesAsync(userId);

            //assert
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
            result.Should().BeOfType<List<IncomeDto>>();
        }

        [Fact]
        public async Task RetrieveIncomesAsync_ShouldReturnIncomeDtoType_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var incomes = new List<Income>
            {
                new Income { Id = 1, UserId = userId, Amount = 100 },
                new Income { Id = 2, UserId = userId, Amount = 200 }
            };
            var mappedIncomes = new List<IncomeDto>
            {
                new IncomeDto { Id = incomes.First().Id, UserId = incomes.First().UserId, Amount = incomes.First().Amount },
                new IncomeDto { Id = incomes[1].Id, UserId = incomes[1].UserId, Amount = incomes[1].Amount }
            };

            _incomeRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                                 .ReturnsAsync(incomes);
            _incomeMapperMock.Setup(mapper => mapper.MapElements(incomes))
                              .Returns(mappedIncomes);

            //act
            var result = await _incomeService.RetrieveIncomesAsync(userId);

            //assert
            result.Should().BeOfType<List<IncomeDto>>();
        }

        [Fact]
        public async Task RetrieveIncomesAsync_ShouldCallRepositoryOnceAndCallMapperOnce_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var incomes = new List<Income>
            {
                new Income { Id = 1, UserId = userId, Amount = 100 },
                new Income { Id = 2, UserId = userId, Amount = 200 }
            };
            var mappedIncomes = new List<IncomeDto>
            {
                new IncomeDto { Id = incomes.First().Id, UserId = incomes.First().UserId, Amount = incomes.First().Amount },
                new IncomeDto { Id = incomes[1].Id, UserId = incomes[1].UserId, Amount = incomes[1].Amount }
            };

            _incomeRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                                 .ReturnsAsync(incomes);
            _incomeMapperMock.Setup(mapper => mapper.MapElements(incomes))
                              .Returns(mappedIncomes);

            //act
            var result = await _incomeService.RetrieveIncomesAsync(userId);

            //assert
            _incomeRepositoryMock.Verify(repo => repo.GetAllAsync(userId), Times.Once);
            _incomeMapperMock.Verify(mapper => mapper.MapElements(incomes), Times.Once);
        }

        [Fact]
        public async Task RetrieveIncomeAsync_ShouldReturnValidIncomeDto_WhenIdIsValidAndUserIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var income = new Income { Id = 1, UserId = userId, Amount = 100, Date = DateTime.Now, Name = "Name" };
            var mappedIncome = new IncomeDto { Id = income.Id, UserId = income.UserId, Amount = income.Amount, Date = income.Date, Name = income.Name };

            _incomeRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                                  .ReturnsAsync(income);
            _incomeMapperMock.Setup(mapper => mapper.Map(income))
                             .Returns(mappedIncome);

            //act
            var result = await _incomeService.RetrieveIncomeAsync(1, userId);

            //assert
            result.Should().BeOfType<IncomeDto>();
            result.Id.Should().Be(1);
            result.UserId.Should().Be(userId);
            result.Amount.Should().Be(100);
            result.Name.Should().Be("Name");
        }

        [Fact]
        public async Task RetrieveIncomeAsync_ShouldThrowIncomeNotFoundException_WhenIdIsValidAndUserIdIsInvalid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var income = new Income { Id = 1, UserId = userId, Amount = 100, Date = DateTime.Now, Name = "Name" };
            var mappedIncome = new IncomeDto { Id = income.Id, UserId = income.UserId, Amount = income.Amount, Date = income.Date, Name = income.Name };

            _incomeRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                                  .ReturnsAsync(income);
            _incomeMapperMock.Setup(mapper => mapper.Map(income))
                             .Returns(mappedIncome);
            var newUserID = Guid.NewGuid();

            //act & assert
            await _incomeService
                 .Invoking(async service => await service.RetrieveIncomeAsync(1, newUserID))
                 .Should()
                 .ThrowAsync<IncomeNotFoundException>()
                 .WithMessage($"Income not found. Id:{1}, userId:{newUserID}.");
        }

        [Fact]
        public async Task RetrieveIncomeAsync_ShouldThrowIncomeNotFoundException_WhenIdIsInValidAndUserIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var income = new Income { Id = 1, UserId = userId, Amount = 100, Date = DateTime.Now, Name = "Name" };
            var mappedIncome = new IncomeDto { Id = income.Id, UserId = income.UserId, Amount = income.Amount, Date = income.Date, Name = income.Name };

            _incomeRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                                  .ReturnsAsync(income);
            _incomeMapperMock.Setup(mapper => mapper.Map(income))
                             .Returns(mappedIncome);

            //act & assert
            await _incomeService
                 .Invoking(async service => await service.RetrieveIncomeAsync(2, userId))
                 .Should()
                 .ThrowAsync<IncomeNotFoundException>()
                 .WithMessage($"Income not found. Id:{2}, userId:{userId}.");
        }

        [Fact]
        public async Task RetrieveIncomeAsync_ShouldCallRepositoryOnceAndCallMapperOnce_WhenIdIsValidAndUserIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var income = new Income { Id = 1, UserId = userId, Amount = 100, Date = DateTime.Now, Name = "Name" };
            var mappedIncome = new IncomeDto { Id = income.Id, UserId = income.UserId, Amount = income.Amount, Date = income.Date, Name = income.Name };

            _incomeRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                                  .ReturnsAsync(income);
            _incomeMapperMock.Setup(mapper => mapper.Map(income))
                             .Returns(mappedIncome);

            //act
            var result = await _incomeService.RetrieveIncomeAsync(1, userId);

            //assert
            _incomeRepositoryMock.Verify(repo => repo.GetAsync(1, userId), Times.Once);
            _incomeMapperMock.Verify(mapper => mapper.Map(income), Times.Once);
        }

        [Fact]
        public async Task AddIncomeAsync_ShouldReturnNewIncome_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var addIncomeDto = new AddIncomeDto { UserId = userId, Amount = 100, Date = DateTime.Now, Name = "Name" };
            var income = new Income { Id = 1, UserId = addIncomeDto.UserId, Amount = addIncomeDto.Amount, Date = addIncomeDto.Date, Name = addIncomeDto.Name };
            var incomeDto = new IncomeDto { Id = income.Id, UserId = income.UserId, Amount = income.Amount, Date = income.Date, Name = income.Name };

            _incomeMapperMock.Setup(mapper => mapper.Map(addIncomeDto)).Returns(income);
            _incomeMapperMock.Setup(mapper => mapper.Map(income)).Returns(incomeDto);
            _incomeRepositoryMock.Setup(repo => repo.AddAsync(income)).ReturnsAsync(income);

            //act
            var result = await _incomeService.AddIncomeAsync(addIncomeDto);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<IncomeDto>();
            result.Id.Should().Be(1);
            result.UserId.Should().Be(userId);
            result.Amount.Should().Be(100);
            result.Name.Should().Be("Name");
        }

        [Fact]
        public async Task AddIncomeAsync_ShouldCallRepositoryOnceAndCallMappersTwice_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var addIncomeDto = new AddIncomeDto { UserId = userId, Amount = 100, Date = DateTime.Now, Name = "Name" };
            var income = new Income { Id = 1, UserId = addIncomeDto.UserId, Amount = addIncomeDto.Amount, Date = addIncomeDto.Date, Name = addIncomeDto.Name };
            var incomeDto = new IncomeDto { Id = income.Id, UserId = income.UserId, Amount = income.Amount, Date = income.Date, Name = income.Name };

            _incomeMapperMock.Setup(mapper => mapper.Map(addIncomeDto)).Returns(income);
            _incomeMapperMock.Setup(mapper => mapper.Map(income)).Returns(incomeDto);
            _incomeRepositoryMock.Setup(repo => repo.AddAsync(income)).ReturnsAsync(income);

            //act
            var result = await _incomeService.AddIncomeAsync(addIncomeDto);

            //assert
            _incomeMapperMock.Verify(mapper => mapper.Map(addIncomeDto), Times.Once);
            _incomeMapperMock.Verify(mapper => mapper.Map(income), Times.Once);
            _incomeRepositoryMock.Verify(repo => repo.AddAsync(income), Times.Once);
        }

        [Fact]
        public async Task AddIncomeAsync_ShouldThrowNullPointerException_WhenIncomeIsNull()
        {
            //arrange
            var addIncomeDto = new AddIncomeDto { };
            addIncomeDto = null;

            //act & assert
            await _incomeService
                .Invoking(async service => await service.AddIncomeAsync(addIncomeDto))
                .Should()
                .ThrowAsync<NullPointerException>()
                .WithMessage("Object is null.");
        }

        [Theory]
        [InlineData("12")]
        [InlineData("This is 51 chatacters long string. dasdadadadadasdd")]
        public async Task AddIncomeAsync_ShouldThrowBadStringLengthException_WhenNameLenghtIsInValid(string text)
        {
            //arrange
            var userId = Guid.NewGuid();
            var incomeDto = new AddIncomeDto { UserId = userId, Amount = 100, Date = DateTime.Now, Name = text };

            //act & assert
            await _incomeService
                .Invoking(async service => await service.AddIncomeAsync(incomeDto))
                .Should()
                .ThrowAsync<BadStringLengthException>()
                .WithMessage($"Name should be between 3 and 50 characters. Now is: {incomeDto.Name}.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public async Task AddIncomeAsync_ShouldThrowBadValueException_WhenAmountIsInValid(int amount)
        {
            //arrange
            var userId = Guid.NewGuid();
            var incomeDto = new AddIncomeDto { UserId = userId, Amount = amount, Date = DateTime.Now, Name = "Name" };

            //act & assert
            await _incomeService
                .Invoking(async service => await service.AddIncomeAsync(incomeDto))
                .Should()
                .ThrowAsync<BadValueException>()
                .WithMessage($"Amount should be more than 0. Now is: {incomeDto.Amount}.");
        }

        [Fact]
        public async Task UpdateIncomeAsync_ShouldUpdateIncome_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateIncomeDto = new UpdateIncomeDto { Id = 1, UserId = userId, Amount = 100, Date = DateTime.Now, Name = "Name" };
            var income = new Income { Id = updateIncomeDto.Id, UserId = updateIncomeDto.UserId, Amount = updateIncomeDto.Amount, Date = updateIncomeDto.Date, Name = updateIncomeDto.Name };
            Income caught = null;

            _incomeMapperMock
                .Setup(mapper => mapper.Map(updateIncomeDto))
                .Returns((UpdateIncomeDto dto) => new Income
                {
                    Id = dto.Id,
                    UserId = dto.UserId,
                    Amount = dto.Amount,
                    Date = dto.Date,
                    Name = dto.Name
                });

            _incomeRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Income>()))
                .Callback<Income>(income =>
                {
                    caught = income;
                })
                .Returns(Task.CompletedTask);

            updateIncomeDto.Name = "Name2";
            updateIncomeDto.Amount = 200;

            //act
            await _incomeService.UpdateIncomeAsync(updateIncomeDto);

            //assert
            caught.Name.Should().Be("Name2");
            caught.Amount.Should().Be(200);
            caught.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task UpdateIncomeAsync_ShouldCallMapperOnceAndRepositoryOnce_WhenCalled()
        {
            //arrange
            var updateIncomeDto = new UpdateIncomeDto { Id = 1, UserId = Guid.NewGuid(), Amount = 100, Date = DateTime.Now, Name = "Name" };
            var income = new Income { Id = updateIncomeDto.Id, UserId = updateIncomeDto.UserId, Amount = updateIncomeDto.Amount, Date = updateIncomeDto.Date, Name = updateIncomeDto.Name };

            _incomeMapperMock
                .Setup(mapper => mapper.Map(updateIncomeDto));

            _incomeRepositoryMock
                .Setup(repo => repo.UpdateAsync(income))
                .Returns(Task.CompletedTask);

            //act
            await _incomeService.UpdateIncomeAsync(updateIncomeDto);

            //assert
            _incomeMapperMock.Verify(mapper => mapper.Map(It.IsAny<UpdateIncomeDto>()), Times.Once);
            _incomeRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Income>()), Times.Once);
        }

        [Fact]
        public async Task UpdateIncomeAsync_ShouldThrowNullPointerException_WhenIncomeIsNull()
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateIncomeDto = new UpdateIncomeDto { Id = 1, UserId = userId, Amount = 100, Date = DateTime.Now, Name = "Name" };
            var income = new Income { Id = updateIncomeDto.Id, UserId = updateIncomeDto.UserId, Amount = updateIncomeDto.Amount, Date = updateIncomeDto.Date, Name = updateIncomeDto.Name };
            income = null;
            updateIncomeDto = null;

            _incomeMapperMock
                .Setup(mapper => mapper.Map(updateIncomeDto))
                .Returns(income);

            _incomeRepositoryMock
                .Setup(repo => repo.UpdateAsync(income))
                .Returns(Task.CompletedTask);

            //act & assert
            await _incomeService
                .Invoking(async service => await service.UpdateIncomeAsync(updateIncomeDto))
                .Should()
                .ThrowAsync<NullPointerException>()
                .WithMessage("Object is null.");
        }

        [Theory]
        [InlineData("12")]
        [InlineData("This is 51 chatacters long string. dasdadadadadasdd")]
        public async Task UpdateIncomeAsync_ShouldThrowBadStringLenghtException_WhenNameLenghtIsInValid(string name)
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateIncomeDto = new UpdateIncomeDto { Id = 1, UserId = userId, Amount = 100, Date = DateTime.Now, Name = name };
            var income = new Income { Id = updateIncomeDto.Id, UserId = updateIncomeDto.UserId, Amount = updateIncomeDto.Amount, Date = updateIncomeDto.Date, Name = updateIncomeDto.Name };

            _incomeMapperMock
                .Setup(mapper => mapper.Map(updateIncomeDto))
                .Returns(income);

            _incomeRepositoryMock
                .Setup(repo => repo.UpdateAsync(income))
                .Returns(Task.CompletedTask);

            //act & assert
            await _incomeService
                .Invoking(async service => await service.UpdateIncomeAsync(updateIncomeDto))
                .Should()
                .ThrowAsync<BadStringLengthException>()
                .WithMessage($"Name should be between 3 and 50 characters. Now is: {income.Name}.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public async Task UpdateIncomeAsync_ShouldThrowBadvalueException_WhenAmountIsInValid(int amount)
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateIncomeDto = new UpdateIncomeDto { Id = 1, UserId = userId, Amount = amount, Date = DateTime.Now, Name = "Name" };
            var income = new Income { Id = updateIncomeDto.Id, UserId = updateIncomeDto.UserId, Amount = updateIncomeDto.Amount, Date = updateIncomeDto.Date, Name = updateIncomeDto.Name };

            _incomeMapperMock
                .Setup(mapper => mapper.Map(updateIncomeDto))
                .Returns(income);

            _incomeRepositoryMock
                .Setup(repo => repo.UpdateAsync(income))
                .Returns(Task.CompletedTask);

            //act & assert
            await _incomeService
                .Invoking(async service => await service.UpdateIncomeAsync(updateIncomeDto))
                .Should()
                .ThrowAsync<BadValueException>()
                .WithMessage($"Amount should be more than 0. Now is: {income.Amount}.");
        }

        [Fact]
        public async Task DeleteIncomeAsync_ShouldDeleteIncome_whenIdIsValidAndUserIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var income = new Income { Id = 1, UserId = userId, Amount = 100, Date = DateTime.Now, Name = "Name" };

            _incomeRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                                 .ReturnsAsync(income);

            //act
            await _incomeService.DeleteIncomeAsync(1, userId);

            //assert
            var result = await _incomeService.RetrieveIncomeAsync(1, userId);
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteIncomeAsync_ShouldNotDeleteIncomeWhenIdIsValidAndUserIdIsInValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var invalidUserId = Guid.NewGuid();
            var income = new Income { Id = 1, UserId = userId, Amount = 100, Date = DateTime.Now, Name = "Name" };

            _incomeRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                                 .ReturnsAsync(income);

            //act & assert
            await _incomeService
                .Invoking(async service => await service.DeleteIncomeAsync(1, invalidUserId))
                .Should()
                .ThrowAsync<IncomeNotFoundException>();

            //assert
            _incomeRepositoryMock.Verify(repo => repo.DeleteAsync(income), Times.Never);
        }

        [Fact]
        public async Task DeleteIncomeAsync_ShouldNotDeleteIncome_WhenIdIsInValidAndUserIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var income = new Income { Id = 1, UserId = userId, Amount = 100, Date = DateTime.Now, Name = "Name" };

            _incomeRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                                 .ReturnsAsync(income);

            //act & assert
            await _incomeService
                .Invoking(async service => await service.DeleteIncomeAsync(2, userId))
                .Should()
                .ThrowAsync<IncomeNotFoundException>();

            //assert
            _incomeRepositoryMock.Verify(repo => repo.DeleteAsync(income), Times.Never);
        }

        [Fact]
        public async Task DeleteIncomeAsync_ShouldThrowIncomeNotFoundException_WhenIncomeIsNull()
        {
            //arrange
            var userId = Guid.NewGuid();

            _incomeRepositoryMock.Setup(repo => repo.DeleteAsync(null))
                                 .Returns(Task.CompletedTask);

            //act & assert
            await _incomeService
                .Invoking(async service => await service.DeleteIncomeAsync(1, userId))
                .Should()
                .ThrowAsync<IncomeNotFoundException>()
                .WithMessage($"Income not found. Id:{1}.");
        }

        [Fact]
        public async Task DeleteIncomeAsync_ShouldCallRepositoriyTwice_WhenIdIsValidAndUserIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var income = new Income { Id = 1, UserId = userId, Amount = 100, Date = DateTime.Now, Name = "Name" };

            _incomeRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                                 .ReturnsAsync(income);

            _incomeRepositoryMock.Setup(repo => repo.DeleteAsync(income))
                                 .Returns(Task.CompletedTask);

            //act
            await _incomeService.DeleteIncomeAsync(1, userId);

            //assert
            _incomeRepositoryMock.Verify(repo => repo.DeleteAsync(income), Times.Once);
            _incomeRepositoryMock.Verify(repo => repo.GetAsync(1, userId), Times.Once);
        }

        [Fact]
        public async Task RetrieveIncomesAsync_ShouldReturnIncomeDtoList_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var incomesList = new List<Income>
            { 
                new Income { Id = 1, UserId = userId, Amount = 100, Date = new DateTime(2024,8,18), Name = "Name1" },
                new Income { Id = 2, UserId = userId, Amount = 200, Date = new DateTime(2024,8,8), Name = "Name2" }
            };
            var incomesDtoList = new List<IncomeDto>
            {
                new IncomeDto { Id = 1, UserId = userId, Amount = 100, Date = new DateTime(2024,8,18), Name = "Name1" },
                new IncomeDto { Id = 2, UserId = userId, Amount = 200, Date = new DateTime(2024,8,8), Name = "Name2" }
            };

            _incomeRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<MonthYearModel>(), userId))
                                 .ReturnsAsync(incomesList);
            
            _incomeMapperMock.Setup(mapper => mapper.MapElements(incomesList))
                             .Returns(incomesDtoList);

            //act
            var result = await _incomeService.RetrieveIncomesAsync(8,2024,userId);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<IncomeDto>>();
            result.Should().HaveCount(2);
        }

        [Fact] // ????
        public async Task RetrieveIncomesAsync_ShouldReturnIncomesForValidUser_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var invalidUserId = Guid.NewGuid();
            var incomesList = new List<Income>
            {
                new Income { Id = 1, UserId = userId, Amount = 100, Date = new DateTime(2024,8,18), Name = "Name1" },
                new Income { Id = 2, UserId = userId, Amount = 200, Date = new DateTime(2024,8,8), Name = "Name2" },
                new Income { Id = 3, UserId = invalidUserId, Amount = 300, Date = new DateTime(2024,5,8), Name = "Name3" }
            };
            var incomesDtoList = new List<IncomeDto>
            {
                new IncomeDto { Id = 1, UserId = userId, Amount = 100, Date = new DateTime(2024,8,18), Name = "Name1" },
                new IncomeDto { Id = 2, UserId = userId, Amount = 200, Date = new DateTime(2024,8,8), Name = "Name2" },
                /// symulowana filtracja brak invalidID
            };

            _incomeRepositoryMock.Setup(repo => repo.GetAsync(It.Is<MonthYearModel>(m => m.Month == 8 && m.Year == 2024), userId))
                                 .ReturnsAsync(incomesList);

            _incomeMapperMock.Setup(mapper => mapper.MapElements(incomesList))
                             .Returns(incomesDtoList);

            //act
            var result = await _incomeService.RetrieveIncomesAsync(8, 2024, userId);

            //assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task RetrieveIncomesAsync2_ShouldCallRepositoryOnceAndCallMapperOnce_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var incomesList = new List<Income>
            {
                new Income { Id = 1, UserId = userId, Amount = 100, Date = new DateTime(2024,8,18), Name = "Name1" },
                new Income { Id = 2, UserId = userId, Amount = 200, Date = new DateTime(2024,8,8), Name = "Name2" }
            };
            var incomesDtoList = new List<IncomeDto>
            {
                new IncomeDto { Id = 1, UserId = userId, Amount = 100, Date = new DateTime(2024,8,18), Name = "Name1" },
                new IncomeDto { Id = 2, UserId = userId, Amount = 200, Date = new DateTime(2024,8,8), Name = "Name2" }
            };

            _incomeRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<MonthYearModel>(), userId))
                                 .ReturnsAsync(incomesList);

            _incomeMapperMock.Setup(mapper => mapper.MapElements(incomesList))
                             .Returns(incomesDtoList);

            //act
            await _incomeService.RetrieveIncomesAsync(8, 2024, userId);

            //assert
            _incomeMapperMock.Verify(mapper => mapper.MapElements(incomesList), Times.Once);
            _incomeRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<MonthYearModel>(), userId), Times.Once);
        }
    }
}