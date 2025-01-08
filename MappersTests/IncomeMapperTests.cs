using BudgetManager.Dto.Income;
using BudgetManager.Features.Incomes.Commands;
using BudgetManager.Mappers;
using BudgetManager.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Runtime.InteropServices;

namespace MappersTests
{
    public class IncomeMapperTests
    {
        private readonly IIncomeMapper _mapper;
        public IncomeMapperTests()
        {
            _mapper = new IncomeMapper();
        }
        [Fact]
        public void Map_IncomeDtoToIncome_ShouldReturnIncome_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var income = new IncomeDto
            {
                Id = 1,
                UserId = userId,
                Date = date,
                Name = "Name",
                Amount = 100
            };

            //act
            var result = _mapper.Map(income);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Income>();
            result.Should().BeEquivalentTo(income);
        }
        [Fact]
        public void Map_AddIncomeDtoToIncome_ShouldReturnIncome_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var income = new AddIncomeDto
            {
                UserId = userId,
                Date = date,
                Name = "Name",
                Amount = 100
            };

            //act
            var result = _mapper.Map(income);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Income>();
            result.Should().BeEquivalentTo(income);
        }
        [Fact]
        public void Map_UpdateIncomeDtoToIncome_ShouldReturnIncome_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var income = new UpdateIncomeDto
            {
                UserId = userId,
                Date = date,
                Name = "Name",
                Amount = 100
            };

            //act
            var result = _mapper.Map(income);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Income>();
            result.Should().BeEquivalentTo(income);
        }
        [Fact]
        public void Map_SaveIncomeCommandToAddIncomeDto_ShouldReturnAddIncomeDto_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var income = new SaveIncomeCommand(userId, "Name", 100, date);

            //act
            var result = _mapper.Map(income);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<AddIncomeDto>();
            result.Should().BeEquivalentTo(income);
        }
        [Fact]
        public void Map_UpdateIncomeCommandToUpdateIncomeDto_ShouldReturnUpdateIncomeDto_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var income = new UpdateIncomeCommand(1, userId, "Name", 100, date);

            //act
            var result = _mapper.Map(income);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UpdateIncomeDto>();
            result.Should().BeEquivalentTo(income);
        }
        [Fact]
        public void Map_IncomeToIncomeDto_ShouldReturnIncomeDto_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var income = new Income
            {
                Id = 1,
                UserId = userId,
                Date = date,
                Name = "Name",
                Amount = 100
            };

            //act
            var result = _mapper.Map(income);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<IncomeDto>();
            result.Should().BeEquivalentTo(income);
        }
        [Fact]
        public void MapElements_IncomeListToIncomeDtoList_ShouldReturnIncomeDtoList_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var incomeList = new List<Income>
            {
                new Income {Id = 1, UserId = userId, Date = date, Name = "Name", Amount = 100 },
                new Income {Id = 2, UserId = userId, Date = date, Name = "Name", Amount = 100 },
                new Income {Id = 3, UserId = userId, Date = date, Name = "Name", Amount = 100 }
            };

            //act
            var result = _mapper.MapElements(incomeList);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<IncomeDto>>();
            result.Should().BeEquivalentTo(incomeList);
        }
        [Fact]
        public void MapElements_IncomeDtoListToIncomeList_ShouldReturnIncomeList_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var incomeList = new List<IncomeDto>
            {
                new IncomeDto {Id = 1, UserId = userId, Date = date, Name = "Name", Amount = 100 },
                new IncomeDto {Id = 2, UserId = userId, Date = date, Name = "Name", Amount = 100 },
                new IncomeDto {Id = 3, UserId = userId, Date = date, Name = "Name", Amount = 100 }
            };

            //act
            var result = _mapper.MapElements(incomeList);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Income>>();
            result.Should().BeEquivalentTo(incomeList);
        }
    }
}