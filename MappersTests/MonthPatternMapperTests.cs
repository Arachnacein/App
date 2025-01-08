using BudgetManager.Dto.MonthPattern;
using BudgetManager.Features.MonthPatterns.Commands;
using BudgetManager.Mappers;
using BudgetManager.Models;
using FluentAssertions;

namespace MappersTests
{
    public class MonthPatternMapperTests
    {
        private readonly IMonthPatternMapper _mapper;
        public MonthPatternMapperTests()
        {
            _mapper = new MonthPatternMapper();
        }
        [Fact]
        public void Map_MonthPatternDtoToMonthPattern_ShouldReturnMonthPattern_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var monthPattern = new MonthPatternDto
            {
                Id = 1,
                UserId = userId,
                Date = date,
                PatternId = 1
            };

            //act
            var result = _mapper.Map(monthPattern);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<MonthPattern>();
            result.Should().BeEquivalentTo(monthPattern, options => options.ExcludingMissingMembers());
        }
        [Fact]
        public void Map_AddMonthPatternDtoToMonthPattern_ShouldReturnMonthPattern_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var monthPattern = new AddMonthPatternDto
            {
                UserId = userId,
                Date = date,
                PatternId = 1
            };

            //act
            var result = _mapper.Map(monthPattern);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<MonthPattern>();
            result.Should().BeEquivalentTo(monthPattern, options => options.ExcludingMissingMembers());
        }
        [Fact]
        public void Map_UpdateMonthPatternDtoToMonthPattern_ShouldReturnMonthPattern_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var monthPattern = new UpdateMonthPatternDto
            {
                Id = 1,
                UserId = userId,
                Date = date,
                PatternId = 1
            };

            //act
            var result = _mapper.Map(monthPattern);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<MonthPattern>();
            result.Should().BeEquivalentTo(monthPattern, options => options.ExcludingMissingMembers());
        }
        [Fact]
        public void Map_MonthPatternToMonthPatternDto_ShouldReturnMonthPatternDto_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var monthPattern = new MonthPattern
            {
                Id = 1,
                UserId = userId,
                Date = date,
                PatternId = 1
            };

            //act
            var result = _mapper.Map(monthPattern);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<MonthPatternDto>();
            result.Should().BeEquivalentTo(monthPattern, options => options.ExcludingMissingMembers());
        }
        [Fact]
        public void Map_SaveMonthPatternCommandToAddMonthPatternDto_ShouldReturnAddMonthMatternDto_WhendCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var monthPattern = new SaveMonthPatternCommand
                (userId, date, 1);

            //act
            var result = _mapper.Map(monthPattern);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<AddMonthPatternDto>();
            result.Should().BeEquivalentTo(monthPattern, options => options.ExcludingMissingMembers());
        }
        [Fact]
        public void Map_UpdateMonthPatternCommandToUpdateMonthPatternDto_ShoudlReturn_UpdateMonthPatternDto_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var monthPattern = new UpdateMonthPatternCommand
                (1, userId, date, 1);

            //act
            var result = _mapper.Map(monthPattern);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UpdateMonthPatternDto>();
            result.Should().BeEquivalentTo(monthPattern, options => options.ExcludingMissingMembers());
        }
        [Fact]
        public void MapElements_MonthPatternListToMonthPatternDtoList_ShouldReturnMonthPatternDtoList_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var monthPatternList = new List<MonthPattern>
            { 
                new MonthPattern { Id = 1, UserId = userId, Date = date, PatternId = 1 },
                new MonthPattern { Id = 2, UserId = userId, Date = date, PatternId = 1 },
                new MonthPattern { Id = 3, UserId = userId, Date = date, PatternId = 1 }
            };

            //act
            var result = _mapper.MapElements(monthPatternList);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<MonthPatternDto>>();
            result.Should().BeEquivalentTo(monthPatternList, options => options.ExcludingMissingMembers());
        }
        [Fact]
        public void MapElements_MonthPatterDtoListToMonthPatternList_ShouldReturnMonthPatternList_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var monthPatternList = new List<MonthPatternDto>
            {
                new MonthPatternDto { Id = 1, UserId = userId, Date = date, PatternId = 1 },
                new MonthPatternDto { Id = 2, UserId = userId, Date = date, PatternId = 1 },
                new MonthPatternDto { Id = 3, UserId = userId, Date = date, PatternId = 1 }
            };

            //act
            var result = _mapper.MapElements(monthPatternList);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<MonthPattern>>();
            result.Should().BeEquivalentTo(monthPatternList, options => options.ExcludingMissingMembers());
        }

    }
}