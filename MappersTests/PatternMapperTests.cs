using BudgetManager.Dto.Pattern;
using BudgetManager.Features.Patterns.Commands;
using BudgetManager.Mappers;
using BudgetManager.Models;
using BudgetManager.Services;
using FluentAssertions;

namespace MappersTests
{
    public class PatternMapperTests
    {
        private readonly IPatternMapper _mapper;
        public PatternMapperTests()
        {
            _mapper = new PatternMapper();
        }
        [Fact]
        public async Task Map_PatternToPatternDto_ShouldReturnPatternDto_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var pattern = new Pattern
            {
                Id = 1,
                UserId = userId,
                Name = "Name",
                Value_Entertainment = 30,
                Value_Fees = 30,
                Value_Saves = 30,
                MonthPatterns = new List<MonthPattern>()
            };

            //act
            var mappedPattern = _mapper.Map(pattern);

            //assert
            mappedPattern.Should().NotBeNull();
            mappedPattern.Should().BeOfType<PatternDto>();
            mappedPattern.Should().BeEquivalentTo(pattern, options => options.Excluding(x => x.MonthPatterns));
        }
        [Fact]
        public async Task Map_PatternDtoToPattern_ShouldReturnPattern_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var patternDto = new PatternDto
            {
                Id = 1,
                UserId = userId,
                Name = "Name",
                Value_Entertainment = 30,
                Value_Fees = 30,
                Value_Saves = 30
            };

            //act
            var mappedPattern = _mapper.Map(patternDto);

            //assert
            mappedPattern.Should().NotBeNull();
            mappedPattern.Should().BeOfType<Pattern>();
            mappedPattern.Should().BeEquivalentTo(patternDto);
        }
        [Fact]
        public async Task Map_AddPatternDtoToPattern_ShouldReturnPattern_WhenCalled()
        {            
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var patternDto = new AddPatternDto
            {
                UserId = userId,
                Name = "Name",
                Value_Entertainment = 30,
                Value_Fees = 30,
                Value_Saves = 30
            };

            //act
            var mappedPattern = _mapper.Map(patternDto);

            //assert
            mappedPattern.Should().NotBeNull();
            mappedPattern.Should().BeOfType<Pattern>();
            mappedPattern.Should().BeEquivalentTo(patternDto);
        }
        [Fact]
        public async Task Map_SavePatternCommandToAddPatternDto_ShouldReturnAddPatternDto_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var savePatternCommand = new SavePatternCommand(userId, "Name", 30, 30, 40);

            //act
            var mappedPattern = _mapper.Map(savePatternCommand);

            //assert
            mappedPattern.Should().NotBeNull();
            mappedPattern.Should().BeOfType<AddPatternDto>();
            mappedPattern.Should().BeEquivalentTo(savePatternCommand);
        }
        [Fact]
        public async Task MapElements_PatternListToPatternDtoList_ShouldReturnPatternDtoList_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var patternList = new List<Pattern>
            {
                new Pattern{ Id = 1,UserId = userId,Name = "Name",Value_Entertainment = 30,Value_Fees = 30,Value_Saves = 40},
                new Pattern{ Id = 2,UserId = userId,Name = "Name",Value_Entertainment = 30,Value_Fees = 30,Value_Saves = 40},
                new Pattern{ Id = 3,UserId = userId,Name = "Name",Value_Entertainment = 30,Value_Fees = 30,Value_Saves = 40}
            };

            //act
            var mappedPatternsList = _mapper.MapElements(patternList);

            //assert
            mappedPatternsList.Should().NotBeNull();
            mappedPatternsList.Should().BeOfType<List<PatternDto>>();
            mappedPatternsList.Should().BeEquivalentTo(patternList, options => options.ExcludingMissingMembers());
        }
        [Fact]
        public async Task MapElements_PatternDtoListToPatternList_ShouldReturnPatternList_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var patternDtoList = new List<PatternDto>
            {
                new PatternDto { Id = 1,UserId = userId,Name = "Name",Value_Entertainment = 30,Value_Fees = 30,Value_Saves = 40},
                new PatternDto { Id = 2,UserId = userId,Name = "Name",Value_Entertainment = 30,Value_Fees = 30,Value_Saves = 40},
                new PatternDto { Id = 3,UserId = userId,Name = "Name",Value_Entertainment = 30,Value_Fees = 30,Value_Saves = 40}
            };

            //act
            var mappedPatternsList = _mapper.MapElements(patternDtoList);

            //assert
            mappedPatternsList.Should().NotBeNull();
            mappedPatternsList.Should().BeOfType<List<Pattern>>();
            mappedPatternsList.Should().BeEquivalentTo(patternDtoList, options => options.ExcludingMissingMembers());
        }
    }
}