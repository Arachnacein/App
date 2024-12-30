using BudgetManager.Dto.Pattern;
using BudgetManager.Exceptions;
using BudgetManager.Exceptions.PatternExceptions;
using BudgetManager.Mappers;
using BudgetManager.Models;
using BudgetManager.Repositories;
using BudgetManager.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Newtonsoft.Json.Linq;

namespace ServicesTests
{
    public class PatternServicesTests
    {
        private readonly Mock<IPatternRepository> _patternRepositoryMock;
        private readonly Mock<IPatternMapper> _patternMapperMock;
        private readonly PatternService _patternService;

        public PatternServicesTests()
        {
            _patternMapperMock = new Mock<IPatternMapper>();
            _patternRepositoryMock = new Mock<IPatternRepository>();
            _patternService = new PatternService(_patternRepositoryMock.Object, _patternMapperMock.Object);
        }

        [Fact]
        public async Task RetrievePatternsAsync_ShouldReturnPatternDtoList_UserIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var patternList = new List<Pattern>
            {
                new Pattern { Id = 1, UserId = userId, Name = "Patern1", Value_Saves = 30, Value_Fees = 30, Value_Entertainment = 40 },
                new Pattern { Id = 2, UserId = userId, Name = "Patern2", Value_Saves = 40, Value_Fees = 30, Value_Entertainment = 30 },
            };
            var patternDtoList = new List<PatternDto>
            {
                new PatternDto { Id = patternList[0].Id, UserId = patternList[0].UserId, Name = patternList[0].Name, Value_Saves = patternList[0].Value_Saves, Value_Fees = patternList[0].Value_Fees, Value_Entertainment = patternList[0].Value_Entertainment },
                new PatternDto { Id = patternList[1].Id, UserId = patternList[1].UserId, Name = patternList[1].Name, Value_Saves = patternList[1].Value_Saves, Value_Fees = patternList[1].Value_Fees, Value_Entertainment = patternList[1].Value_Entertainment }
            };

            _patternRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                .ReturnsAsync(patternList);
            _patternMapperMock.Setup(mapper => mapper.MapElements(patternList))
                .Returns(patternDtoList);

            //act
            var result = await _patternService.RetrievePatternsAsync(userId);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<PatternDto>>();
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task RetrievePatternsAsync_ShouldReturnEmptyList_WhenPatternsNotFound()
        {
            //arrange
            var userId = Guid.NewGuid();
            var patternList = new List<Pattern> { };
            var patternDtoList = new List<PatternDto> { };

            _patternRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                .ReturnsAsync(patternList);
            _patternMapperMock.Setup(mapper => mapper.MapElements(patternList))
                .Returns(patternDtoList);

            //act
            var result = await _patternService.RetrievePatternsAsync(userId);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<PatternDto>>();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task RetrievePatternsAsync_ShouldCallRepositoryOnceAndCallMapperOnce()
        {
            //arrange
            var userId = Guid.NewGuid();
            var patternList = new List<Pattern>
            {
                new Pattern { Id = 1, UserId = userId, Name = "Patern1", Value_Saves = 30, Value_Fees = 30, Value_Entertainment = 40 },
                new Pattern { Id = 2, UserId = userId, Name = "Patern2", Value_Saves = 40, Value_Fees = 30, Value_Entertainment = 30 },
            };
            var patternDtoList = new List<PatternDto>
            {
                new PatternDto { Id = patternList[0].Id, UserId = patternList[0].UserId, Name = patternList[0].Name, Value_Saves = patternList[0].Value_Saves, Value_Fees = patternList[0].Value_Fees, Value_Entertainment = patternList[0].Value_Entertainment },
                new PatternDto { Id = patternList[1].Id, UserId = patternList[1].UserId, Name = patternList[1].Name, Value_Saves = patternList[1].Value_Saves, Value_Fees = patternList[1].Value_Fees, Value_Entertainment = patternList[1].Value_Entertainment }
            };

            _patternRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                .ReturnsAsync(patternList);
            _patternMapperMock.Setup(mapper => mapper.MapElements(patternList))
                .Returns(patternDtoList);

            //act
            var result = await _patternService.RetrievePatternsAsync(userId);

            //assert
            _patternMapperMock.Verify(mapper => mapper.MapElements(patternList), Times.Once);
            _patternRepositoryMock.Verify(repo => repo.GetAllAsync(userId), Times.Once);
        }

        [Fact]
        public async Task RetrievePatternAsync_ShouldReturnPatternDto_WhenIdIsValidAndUserIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var pattern = new Pattern
            {
                Id = 1,
                UserId = userId,
                Name = "Patern1",
                Value_Saves = 30,
                Value_Fees = 30,
                Value_Entertainment = 40
            };
            var patternDto = new PatternDto
            {
                Id = pattern.Id,
                UserId = pattern.UserId,
                Name = pattern.Name,
                Value_Saves = pattern.Value_Saves,
                Value_Fees = pattern.Value_Fees,
                Value_Entertainment = pattern.Value_Entertainment
            };

            _patternRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                .ReturnsAsync(pattern);
            _patternMapperMock.Setup(mapper => mapper.Map(pattern))
                .Returns(patternDto);

            //act
            var result = await _patternService.RetrievePatternAsync(1, userId);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PatternDto>();
        }
        
        [Fact]
        public async Task RetrievePatternAsync_ShouldThrowPatternNotFoundEception_WhenIdIsValidAndUsrIdIsInValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var pattern = new Pattern
            {
                Id = 1,
                UserId = userId,
                Name = "Patern1",
                Value_Saves = 30,
                Value_Fees = 30,
                Value_Entertainment = 40
            };
            var patternDto = new PatternDto
            {
                Id = pattern.Id,
                UserId = pattern.UserId,
                Name = pattern.Name,
                Value_Saves = pattern.Value_Saves,
                Value_Fees = pattern.Value_Fees,
                Value_Entertainment = pattern.Value_Entertainment
            };

            _patternRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                .ReturnsAsync(pattern);
            _patternMapperMock.Setup(mapper => mapper.Map(pattern))
                .Returns(patternDto);

            //act & assert
            await _patternService
                .Invoking(async service => await service.RetrievePatternAsync(1, Guid.NewGuid()))
                .Should()
                .ThrowAsync<PatternNotFoundException>()
                .WithMessage($"Pattern not found. Id:{1}");
        }
        
        [Fact]
        public async Task RetrievePatternAsync_ShouldThrowPatternNotFoundEception_WhenIdIsInValidAndUsrIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var pattern = new Pattern
            {
                Id = 1,
                UserId = userId,
                Name = "Patern1",
                Value_Saves = 30,
                Value_Fees = 30,
                Value_Entertainment = 40
            };
            var patternDto = new PatternDto
            {
                Id = pattern.Id,
                UserId = pattern.UserId,
                Name = pattern.Name,
                Value_Saves = pattern.Value_Saves,
                Value_Fees = pattern.Value_Fees,
                Value_Entertainment = pattern.Value_Entertainment
            };

            _patternRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                .ReturnsAsync(pattern);
            _patternMapperMock.Setup(mapper => mapper.Map(pattern))
                .Returns(patternDto);

            //act & assert
            await _patternService
                .Invoking(async service => await service.RetrievePatternAsync(2, userId))
                .Should()
                .ThrowAsync<PatternNotFoundException>()
                .WithMessage($"Pattern not found. Id:{2}");


        }
        
        [Fact]
        public async Task RetrievePatternAsync_ShouldCallRepositoryOnceAndMapperOnce_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var pattern = new Pattern
            {
                Id = 1,
                UserId = userId,
                Name = "Patern1",
                Value_Saves = 30,
                Value_Fees = 30,
                Value_Entertainment = 40
            };
            var patternDto = new PatternDto
            {
                Id = pattern.Id,
                UserId = pattern.UserId,
                Name = pattern.Name,
                Value_Saves = pattern.Value_Saves,
                Value_Fees = pattern.Value_Fees,
                Value_Entertainment = pattern.Value_Entertainment
            };

            _patternRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                .ReturnsAsync(pattern);
            _patternMapperMock.Setup(mapper => mapper.Map(pattern))
                .Returns(patternDto);

            //act
            var result = await _patternService.RetrievePatternAsync(1, userId);

            //assert

            _patternMapperMock.Verify(mapper => mapper.Map(pattern), Times.Once);
            _patternRepositoryMock.Verify(repo => repo.GetAsync(1, userId), Times.Once);
        }

        [Fact]
        public async Task AddPatternAsync_ShouldThrowArgumentNullException_WhenModelIsNull()
        {
            //arrange
            var userId = Guid.NewGuid();
            var addPatternDto = new AddPatternDto { };
            addPatternDto = null;

            //act & assert
            await _patternService
                .Invoking(async service => await service.AddPatternAsync(addPatternDto))
                .Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [InlineData("123", "Name have incorrect length. Should be more than 3 characters.")]
        [InlineData("This is 51 chatacters long string. dasdadadadadasdd", "Name have incorrect length. Should be less than 50 characters.")]
        public async Task AddPatternAsync_ShouldThrowBadStringLengthException_WhenNameIsInValidLength(string name, string message)
        {
            //arrange
            var userId = Guid.NewGuid();
            var addPatternDto = new AddPatternDto
            {
                UserId = userId,
                Name = name,
                Value_Saves = 30,
                Value_Fees = 30,
                Value_Entertainment = 40
            };

            //act & assert
            await _patternService
                .Invoking(async service => await service.AddPatternAsync(addPatternDto))
                .Should()
                .ThrowAsync<BadStringLengthException>()
                .WithMessage(message);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public async Task AddPatternAsync_ShouldThrowBadValueException_WhenValueFeesHaveInValidValue(double value)
        {
            //arrange
            var userId = Guid.NewGuid();
            var addPatternDto = new AddPatternDto
            {
                UserId = userId,
                Name = "Name",
                Value_Saves = 30,
                Value_Fees = value,
                Value_Entertainment = 40
            };

            //act & assert
            await _patternService
                .Invoking(async service => await service.AddPatternAsync(addPatternDto))
                .Should()
                .ThrowAsync<BadValueException>()
                .WithMessage($"Fees Value should be more than 0 and less than 100. ({value})");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public async Task AddPatternAsync_ShouldThrowBadValueException_WhenValueSavesHaveInValidValue(double value)
        {
            //arrange
            var userId = Guid.NewGuid();
            var addPatternDto = new AddPatternDto
            {
                UserId = userId,
                Name = "Name",
                Value_Saves = value,
                Value_Fees = 30,
                Value_Entertainment = 40
            };

            //act & assert
            await _patternService
                .Invoking(async service => await service.AddPatternAsync(addPatternDto))
                .Should()
                .ThrowAsync<BadValueException>()
                .WithMessage($"Saves Value should be more than 0 and less than 100. ({value})");
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public async Task AddPatternAsync_ShouldThrowBadValueException_WhenValueEntertainmentHaveInValidValue(double value)
        {
            //arrange
            var userId = Guid.NewGuid();
            var addPatternDto = new AddPatternDto
            {
                UserId = userId,
                Name = "Name",
                Value_Saves = 30,
                Value_Fees = 30,
                Value_Entertainment = value
            };

            //act & assert
            await _patternService
                .Invoking(async service => await service.AddPatternAsync(addPatternDto))
                .Should()
                .ThrowAsync<BadValueException>()
                .WithMessage($"Entertainment Value should be more than 0 and less than 100. ({value})");
        }

        [Theory]
        [InlineData(0,0,0)]
        [InlineData(30,40,0)]
        [InlineData(1,99,25)]
        public async Task AddPatternAsync_ShouldThrowBadValueException_WhenSumOfValueFeesAndValueSavesAndValueEntertainmentIsNot100(int valueSaves, int valueFees, int valueEntertainment)
        {
            //arrange
            var userId = Guid.NewGuid();
            var addPatternDto = new AddPatternDto
            {
                UserId = userId,
                Name = "Name",
                Value_Saves = valueSaves,
                Value_Fees = valueFees,
                Value_Entertainment = valueEntertainment
            };

            var sum = addPatternDto.Value_Saves + addPatternDto.Value_Fees + addPatternDto.Value_Entertainment;

            //act & assert
            await _patternService
                .Invoking(async service => await service.AddPatternAsync(addPatternDto))
                .Should()
                .ThrowAsync<BadValueException>()
                .WithMessage($"Value_Fees + Value_Saves + Value_Entertainment Should be 100%. Current is {sum}.");
        }

        [Fact]
        public async Task AddPatternAsync_ShouldCallRepositoryOneAndCallMapperTwice()
        {
            //arrange
            var userId = Guid.NewGuid();
            var addPattern = new Pattern
            {
                UserId = userId,
                Name = "Patern1",
                Value_Saves = 30,
                Value_Fees = 30,
                Value_Entertainment = 40
            };
            var addPatternDto = new AddPatternDto
            {
                UserId = addPattern.UserId,
                Name = addPattern.Name,
                Value_Saves = addPattern.Value_Saves,
                Value_Fees = addPattern.Value_Fees,
                Value_Entertainment = addPattern.Value_Entertainment
            };            
            var patternDto = new PatternDto
            {
                Id = 1,
                UserId = addPattern.UserId,
                Name = addPattern.Name,
                Value_Saves = addPattern.Value_Saves,
                Value_Fees = addPattern.Value_Fees,
                Value_Entertainment = addPattern.Value_Entertainment
            };

            _patternMapperMock.Setup(mapper => mapper.Map(addPatternDto))
                .Returns(addPattern);
            _patternRepositoryMock.Setup(repo => repo.AddAsync(addPattern));
            _patternMapperMock.Setup(mapper => mapper.Map(addPattern))
                .Returns(patternDto);

            //act
            await _patternService.AddPatternAsync(addPatternDto);

            //assert
            _patternRepositoryMock.Verify(repo => repo.AddAsync(addPattern), Times.Once);
            _patternMapperMock.Verify(mapper => mapper.Map(addPattern), Times.Once);
            _patternMapperMock.Verify(mapper => mapper.Map(addPatternDto), Times.Once);
        }

        [Fact]
        public async Task AddPatternAsync_ShouldReturnPatternDto_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var addPattern = new Pattern
            {
                UserId = userId,
                Name = "Pattern1",
                Value_Saves = 30,
                Value_Fees = 30,
                Value_Entertainment = 40
            };
            var addPatternDto = new AddPatternDto
            {
                UserId = addPattern.UserId,
                Name = addPattern.Name,
                Value_Saves = addPattern.Value_Saves,
                Value_Fees = addPattern.Value_Fees,
                Value_Entertainment = addPattern.Value_Entertainment
            };
            var patternDto = new PatternDto
            {
                Id = 1,
                UserId = addPattern.UserId,
                Name = addPattern.Name,
                Value_Saves = addPattern.Value_Saves,
                Value_Fees = addPattern.Value_Fees,
                Value_Entertainment = addPattern.Value_Entertainment
            };

            _patternMapperMock.Setup(mapper => mapper.Map(addPatternDto))
                .Returns(addPattern);
            _patternRepositoryMock.Setup(repo => repo.AddAsync(addPattern));
            _patternMapperMock.Setup(mapper => mapper.Map(addPattern))
                .Returns(patternDto);

            //act
            var result = await _patternService.AddPatternAsync(addPatternDto);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PatternDto>();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Pattern1");
            result.Value_Entertainment.Should().Be(40);
            result.Value_Fees.Should().Be(30);
            result.Value_Saves.Should().Be(30);

        }

        [Fact]
        public async Task DeletePatternAsync_ShouldCallRepositoryTwice_WhenCalled()
        {            
            //arrange
            var userId = Guid.NewGuid();
            var pattern = new Pattern
            {
                Id = 1,
                UserId = userId,
                Name = "Pattern1",
                Value_Saves = 30,
                Value_Fees = 30,
                Value_Entertainment = 40
            };

            _patternRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                .ReturnsAsync(pattern);
            _patternRepositoryMock.Setup(repo => repo.DeleteAsync(1, userId));

            //act
            await _patternService.DeletePatternAsync(1, userId);

            //assert
            _patternRepositoryMock.Verify(repo => repo.GetAsync(1, userId), Times.Once);
            _patternRepositoryMock.Verify(repo => repo.DeleteAsync(1, userId), Times.Once);
        }

        [Fact]
        public async Task DeletePatternAsync_ShouldThrowPatternNotFoundException_WhenIdIsValidAndUserIdIsInValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var pattern = new Pattern
            {
                Id = 1,
                UserId = userId,
                Name = "Pattern1",
                Value_Saves = 30,
                Value_Fees = 30,
                Value_Entertainment = 40
            };

            _patternRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                .ReturnsAsync(pattern);
            _patternRepositoryMock.Setup(repo => repo.DeleteAsync(1, userId));

            //act & assert
            await _patternService
                .Invoking(async service => await service.DeletePatternAsync(1, Guid.NewGuid()))
                .Should()
                .ThrowAsync<PatternNotFoundException>()
                .WithMessage($"Pattern not found. Id:{1}");
        }        
        
        [Fact]
        public async Task DeletePatternAsync_ShouldThrowPatternNotFoundException_WhenIdIsInValidAndUserIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var pattern = new Pattern
            {
                Id = 1,
                UserId = userId,
                Name = "Pattern1",
                Value_Saves = 30,
                Value_Fees = 30,
                Value_Entertainment = 40
            };

            _patternRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                .ReturnsAsync(pattern);
            _patternRepositoryMock.Setup(repo => repo.DeleteAsync(1, userId));

            //act & assert
            await _patternService
                .Invoking(async service => await service.DeletePatternAsync(2, userId))
                .Should()
                .ThrowAsync<PatternNotFoundException>()
                .WithMessage($"Pattern not found. Id:{2}");
        }

        [Fact]
        public async Task DeletePatternAsync_ShouldDeletePattern_WhenDataIsValid ()
        {
            //arrange
            var userId = Guid.NewGuid();
            var pattern = new Pattern
            {
                Id = 1,
                UserId = userId,
                Name = "Pattern1",
                Value_Saves = 30,
                Value_Fees = 30,
                Value_Entertainment = 40
            };

            _patternRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                .ReturnsAsync(pattern);
            _patternRepositoryMock.Setup(repo => repo.DeleteAsync(1, userId));

            //act
            await _patternService.DeletePatternAsync(1, userId);

            //assert
            var result = await _patternService.RetrievePatternAsync(1, userId);
            result.Should().BeNull();
        }}
}