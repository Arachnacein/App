using BudgetManager.Dto;
using BudgetManager.Dto.MonthPattern;
using BudgetManager.Dto.Pattern;
using BudgetManager.Exceptions.PatternExceptions;
using BudgetManager.Mappers;
using BudgetManager.Models;
using BudgetManager.Repositories;
using BudgetManager.Services;
using FluentAssertions;
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
            //arrange
            var userId = Guid.NewGuid();
            var patternId = 1;
            var dateTime = DateTime.Now;
            var monthPattern = new MonthPattern { Id = patternId, UserId = userId, Date = dateTime, PatternId = 2 };
            var monthPatternDto = new MonthPatternDto { Id = monthPattern.Id, UserId = monthPattern.UserId, Date = monthPattern.Date, PatternId = monthPattern.PatternId };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(patternId, userId))
                                 .ReturnsAsync(monthPattern);
            _monthPatternMapperMock.Setup(mapper => mapper.Map(monthPattern))
                              .Returns(monthPatternDto);

            //act
            var result = await _monthPatternService.RetrieveMonthPatternAsync(patternId, userId);

            //assert

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.UserId.Should().Be(userId);
            result.Date.Should().Be(dateTime);
        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldReturnMonthPatternDto_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var patternId = 1;
            var dateTime = DateTime.Now;
            var monthPattern = new MonthPattern { };
            var monthPatternDto = new MonthPatternDto { };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(patternId, userId))
                                 .ReturnsAsync(monthPattern);
            _monthPatternMapperMock.Setup(mapper => mapper.Map(monthPattern))
                              .Returns(monthPatternDto);

            //act
            var result = await _monthPatternService.RetrieveMonthPatternAsync(patternId, userId);

            //assert
            result.Should().BeOfType<MonthPatternDto>();
        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldThrowPatternNotFoundException_WhenIdIsInValidAndUserIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var patternId = 1;
            var invalidPatternId = 33;
            var dateTime = DateTime.Now;
            var monthPattern = new MonthPattern { Id = patternId, UserId = userId, Date = dateTime, PatternId = 2 };
            var monthPatternDto = new MonthPatternDto { Id = monthPattern.Id, UserId = monthPattern.UserId, Date = monthPattern.Date, PatternId = monthPattern.PatternId };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(patternId, userId))
                                 .ReturnsAsync(monthPattern);
            _monthPatternMapperMock.Setup(mapper => mapper.Map(monthPattern))
                              .Returns(monthPatternDto);

            //act & assert
            await _monthPatternService
                .Invoking(async service => await service.RetrieveMonthPatternAsync(invalidPatternId, userId))
                .Should()
                .ThrowAsync<PatternNotFoundException>()
                .WithMessage($"Pattern not found exception. Id: {invalidPatternId}.");
        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldThrowPatternNotFoundException_WhenIdIsValidAndUserIdIsInValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var inValidUserId = Guid.NewGuid();
            var patternId = 1;
            var dateTime = DateTime.Now;
            var monthPattern = new MonthPattern { Id = patternId, UserId = userId, Date = dateTime, PatternId = 2 };
            var monthPatternDto = new MonthPatternDto { Id = monthPattern.Id, UserId = monthPattern.UserId, Date = monthPattern.Date, PatternId = monthPattern.PatternId };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(patternId, userId))
                                 .ReturnsAsync(monthPattern);
            _monthPatternMapperMock.Setup(mapper => mapper.Map(monthPattern))
                              .Returns(monthPatternDto);

            //act & assert
            await _monthPatternService
                .Invoking(async service => await service.RetrieveMonthPatternAsync(patternId, inValidUserId))
                .Should()
                .ThrowAsync<PatternNotFoundException>()
                .WithMessage($"Pattern not found exception. Id: {patternId}.");
        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldCallRepositoryOnceAndMapperOnce_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var patternId = 1;
            var monthPattern = new MonthPattern { };
            var monthPatternDto = new MonthPatternDto { };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(patternId, userId))
                                 .ReturnsAsync(monthPattern);
            _monthPatternMapperMock.Setup(mapper => mapper.Map(monthPattern))
                              .Returns(monthPatternDto);

            //act
            await _monthPatternService.RetrieveMonthPatternAsync(patternId, userId);

            //assert
            _monthPatternMapperMock.Verify(mapper => mapper.Map(monthPattern), Times.Once);
            _monthPatternRepositoryMock.Verify(repo => repo.GetAsync(patternId, userId), Times.Once);
        }

        [Fact]
        public async Task RetrieveMonthPatternsAsync_ShouldReturnList_WhenUserIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var datetTime = DateTime.Now;
            var monthPatternList = new List<MonthPattern>
            {
                new MonthPattern { Id = 1, UserId = userId, Date = datetTime, PatternId = 2 },
                new MonthPattern { Id = 2, UserId = userId, Date = datetTime, PatternId = 3 }
            };
            var monthPatternDtoList = new List<MonthPatternDto>
            {
                new MonthPatternDto { Id = monthPatternList[0].Id, UserId = monthPatternList[0].UserId, Date = monthPatternList[0].Date, PatternId = monthPatternList[0].PatternId },
                new MonthPatternDto { Id = monthPatternList[1].Id, UserId = monthPatternList[1].UserId, Date = monthPatternList[1].Date, PatternId = monthPatternList[1].PatternId }
            };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                                 .ReturnsAsync(monthPatternList);
            _monthPatternMapperMock.Setup(mapper => mapper.MapElements(monthPatternList))
                              .Returns(monthPatternDtoList);

            //act
            var result = await _monthPatternService.RetrieveMonthPatternsAsync(userId);

            //assert

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeOfType<List<MonthPatternDto>>();
        }

        [Fact]
        public async Task RetrieveMonthPatternsAsync_ShouldReturnEmptyList_WhenNotFoundAnyMonthPatterns()
        {
            //arrange
            var userId = Guid.NewGuid();
            var datetTime = DateTime.Now;
            var monthPatternList = new List<MonthPattern>
            {
            };
            var monthPatternDtoList = new List<MonthPatternDto>
            {
            };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                                 .ReturnsAsync(monthPatternList);
            _monthPatternMapperMock.Setup(mapper => mapper.MapElements(monthPatternList))
                              .Returns(monthPatternDtoList);

            //act
            var result = await _monthPatternService.RetrieveMonthPatternsAsync(userId);

            //assert
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task RetrieveMonthPatternsAsync_ShouldCallRepositoryOnceAndCallMapperOnce()
        {
            //arrange
            var userId = Guid.NewGuid();
            var datetTime = DateTime.Now;
            var monthPatternList = new List<MonthPattern>
            {
            };
            var monthPatternDtoList = new List<MonthPatternDto>
            {
            };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                                 .ReturnsAsync(monthPatternList);
            _monthPatternMapperMock.Setup(mapper => mapper.MapElements(monthPatternList))
                              .Returns(monthPatternDtoList);

            //act
            var result = await _monthPatternService.RetrieveMonthPatternsAsync(userId);

            //assert
            _monthPatternMapperMock.Verify(mapper => mapper.MapElements(monthPatternList), Times.Once);
            _monthPatternRepositoryMock.Verify(repo => repo.GetAllAsync(userId), Times.Once);
        }

        [Fact]
        public async Task AddMonthPatternAsync_ShouldAddMonthPattern_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var addMonthPatternDto = new AddMonthPatternDto
            {
                UserId = userId,
                Date = new DateTime(2025, 1, 1),
                PatternId = 1
            };

            var checkPattern = new Pattern { Id = 1, UserId = userId };
            var mappedMonthPattern = new MonthPattern { UserId = userId, Date = addMonthPatternDto.Date, PatternId = addMonthPatternDto.PatternId };
            var resultMonthPatternDto = new MonthPatternDto { UserId = userId, Date = addMonthPatternDto.Date, PatternId = addMonthPatternDto.PatternId };

            _patternRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                                  .ReturnsAsync(checkPattern);
            _monthPatternRepositoryMock.Setup(repo => repo.CheckExistsAsync(It.IsAny<MonthYearModel>(), userId))
                                        .ReturnsAsync(0);
            _monthPatternMapperMock.Setup(mapper => mapper.Map(addMonthPatternDto))
                                   .Returns(mappedMonthPattern);
            _monthPatternMapperMock.Setup(mapper => mapper.Map(mappedMonthPattern))
                                   .Returns(resultMonthPatternDto);

            //act
            var result = await _monthPatternService.AddMonthPatternAsync(addMonthPatternDto);

            //assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.Date.Should().Be(addMonthPatternDto.Date);
            result.PatternId.Should().Be(1);
        }

        [Fact]
        public async Task AddMonthPatternAsync_ShouldThrowPatternNotFoundException_WhenPatternDoesntExists()
        {
            //arrange
            var userId = Guid.NewGuid();
            var addMonthPatternDto = new AddMonthPatternDto
            {
                UserId = userId,
                Date = new DateTime(2025, 1, 1),
                PatternId = 99
            };

            _patternRepositoryMock.Setup(repo => repo.GetAsync(99, userId))
                                  .ReturnsAsync((Pattern)null);

            //act & assert
            await _monthPatternService
                .Invoking(async service => await service.AddMonthPatternAsync(addMonthPatternDto))
                .Should()
                .ThrowAsync<PatternNotFoundException>()
                .WithMessage($"Pattern not found. Id:{99}.");
        }

        [Fact]
        public async Task AddMonthPatternAsync_ShouldThrowMonthPatternAlreadyExistsException_WhenPatternAlreadyExists()
        {
            //arrange
            var userId = Guid.NewGuid();
            var addMonthPatternDto = new AddMonthPatternDto
            {
                UserId = userId,
                Date = new DateTime(2025, 1, 1),
                PatternId = 1
            };
            _patternRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                                  .ReturnsAsync(new Pattern { Id = 1, UserId = userId });
            _monthPatternRepositoryMock.Setup(repo => repo.CheckExistsAsync(It.IsAny<MonthYearModel>(), userId))
                                        .ReturnsAsync(1);

            //act & assert
            await _monthPatternService
                .Invoking(async service => await service.AddMonthPatternAsync(addMonthPatternDto))
                .Should()
                .ThrowAsync<MonthPatternAlreadyExistsException>()
                .WithMessage($"Pattern for Month:{1} and Year:{2025} already exists.");
        }

        [Fact]
        public async Task AddMonthPatternAsync_ShouldCallRepositoryThriceAndCallMapperTwice_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var addMonthPatternDto = new AddMonthPatternDto
            {
                UserId = userId,
                Date = new DateTime(2025, 1, 1),
                PatternId = 1
            };

            var checkPattern = new Pattern { };
            var mappedMonthPattern = new MonthPattern { };
            var resultMonthPatternDto = new MonthPatternDto { };

            _patternRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                                  .ReturnsAsync(checkPattern);
            _monthPatternRepositoryMock.Setup(repo => repo.CheckExistsAsync(It.IsAny<MonthYearModel>(), userId))
                                        .ReturnsAsync(0);
            _monthPatternMapperMock.Setup(mapper => mapper.Map(addMonthPatternDto))
                                   .Returns(mappedMonthPattern);
            _monthPatternMapperMock.Setup(mapper => mapper.Map(mappedMonthPattern))
                                   .Returns(resultMonthPatternDto);

            //act
            var result = await _monthPatternService.AddMonthPatternAsync(addMonthPatternDto);

            //assert
            _patternRepositoryMock.Verify(patternRepo => patternRepo.GetAsync(1, userId), Times.Once);
            _monthPatternRepositoryMock.Verify(monthPatternRepo => monthPatternRepo.CheckExistsAsync(It.IsAny<MonthYearModel>(), userId), Times.Once);
            _monthPatternMapperMock.Verify(mapper => mapper.Map(addMonthPatternDto), Times.Once);
            _monthPatternMapperMock.Verify(mapper => mapper.Map(mappedMonthPattern), Times.Once);
        }

        [Fact]
        public async Task AddMonthPatternAsync_ShouldReturnNewMonthPattern_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var date = new DateTime(2025, 1, 1);
            var addMonthPatternDto = new AddMonthPatternDto
            {
                UserId = userId,
                Date = date,
                PatternId = 1
            };

            var checkPattern = new Pattern { Id = 1, UserId = userId };
            var mappedMonthPattern = new MonthPattern { UserId = userId, Date = addMonthPatternDto.Date, PatternId = addMonthPatternDto.PatternId };
            var resultMonthPatternDto = new MonthPatternDto { UserId = userId, Date = addMonthPatternDto.Date, PatternId = addMonthPatternDto.PatternId };

            _patternRepositoryMock.Setup(repo => repo.GetAsync(1, userId))
                                  .ReturnsAsync(checkPattern);
            _monthPatternRepositoryMock.Setup(repo => repo.CheckExistsAsync(It.IsAny<MonthYearModel>(), userId))
                                        .ReturnsAsync(0);
            _monthPatternMapperMock.Setup(mapper => mapper.Map(addMonthPatternDto))
                                   .Returns(mappedMonthPattern);
            _monthPatternMapperMock.Setup(mapper => mapper.Map(mappedMonthPattern))
                                   .Returns(resultMonthPatternDto);

            //act
            var result = await _monthPatternService.AddMonthPatternAsync(addMonthPatternDto);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<MonthPatternDto>();
            result.Id.Should().NotBe(null);
            result.Date.Should().Be(date);
            result.PatternId.Should().Be(1);
        }

        [Fact]
        public async Task UpdateMonthPatternAsync_ShouldCallRepositoryTwiceAndCallMapperOnce_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateMonthPatternDto = new UpdateMonthPatternDto { Id = 1, UserId = userId, Date = new DateTime(2025, 1, 1), PatternId = 1 };
            var existingMonthPattern = new MonthPattern { Id = 1, UserId = userId, Date = new DateTime(2025, 1, 1), PatternId = 1 };
            var mappedMonthPattern = new MonthPattern { Id = 1, UserId = userId, Date = updateMonthPatternDto.Date, PatternId = updateMonthPatternDto.PatternId };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(updateMonthPatternDto.Id, updateMonthPatternDto.UserId))
                                       .ReturnsAsync(existingMonthPattern);
            _monthPatternMapperMock.Setup(mapper => mapper.Map(updateMonthPatternDto))
                                   .Returns(mappedMonthPattern);

            //act
            await _monthPatternService.UpdateMonthPatternAsync(updateMonthPatternDto);

            //assert
            _monthPatternRepositoryMock.Verify(repo => repo.GetAsync(updateMonthPatternDto.Id, updateMonthPatternDto.UserId), Times.Once);
            _monthPatternMapperMock.Verify(mapper => mapper.Map(updateMonthPatternDto), Times.Once);
            _monthPatternRepositoryMock.Verify(repo => repo.UpdateAsync(mappedMonthPattern), Times.Once);
        }

        [Fact]
        public async Task UpdateMonthPatternAsync_ShouldThrowMonthPatternNotFoundException_WhenIdIsValidAndUserIdIsInValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var invalidUserId = Guid.NewGuid();
            var updateMonthPatternDto = new UpdateMonthPatternDto
            {
                Id = 1,
                UserId = invalidUserId,
                Date = new DateTime(2025, 1, 1),
                PatternId = 1
            };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(updateMonthPatternDto.Id, updateMonthPatternDto.UserId))
                                       .ReturnsAsync((MonthPattern)null);

            //act & assert
            await _monthPatternService
                .Invoking(async service => await service.UpdateMonthPatternAsync(updateMonthPatternDto))
                .Should()
                .ThrowAsync<MonthPatternNotFoundException>()
                .WithMessage($"Pattern not found exception. Id: {1}.");
        }

        [Fact]
        public async Task UpdateMonthPatternAsync_ShouldThrowMonthPatternNotFoundException_WhenIdIsInValidAndUserIdIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var updateMonthPatternDto = new UpdateMonthPatternDto
            {
                Id = 2,
                UserId = userId,
                Date = new DateTime(2025, 1, 1),
                PatternId = 1
            };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(updateMonthPatternDto.Id, updateMonthPatternDto.UserId))
                                       .ReturnsAsync((MonthPattern)null);

            //act & assert
            await _monthPatternService
                .Invoking(async service => await service.UpdateMonthPatternAsync(updateMonthPatternDto))
                .Should()
                .ThrowAsync<MonthPatternNotFoundException>()
                .WithMessage($"Pattern not found exception. Id: {2}.");
        }

        [Fact]
        public async Task UpdateMonthPatternAsync_ShouldUpdateMonthPattern_WhenDataIsValid()
        {
            // arrange
            var userId = Guid.NewGuid();
            var updateMonthPatternDto = new UpdateMonthPatternDto { Id = 1, UserId = userId, Date = new DateTime(2025, 1, 1), PatternId = 1 };
            var existingMonthPattern = new MonthPattern { Id = 1, UserId = userId, Date = new DateTime(2025, 1, 1), PatternId = 1 };
            MonthPattern caught = null;

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(updateMonthPatternDto.Id, updateMonthPatternDto.UserId))
                                       .ReturnsAsync(existingMonthPattern);
            _monthPatternMapperMock.Setup(mapper => mapper.Map(updateMonthPatternDto))
                                   .Returns((UpdateMonthPatternDto dto) => new MonthPattern
                                   {
                                       Id = dto.Id,
                                       UserId = dto.UserId,
                                       Date = dto.Date,
                                       PatternId = dto.PatternId
                                   });
            _monthPatternRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<MonthPattern>()))
                                       .Callback<MonthPattern>(monthPattern =>
                                       {
                                           caught = monthPattern;
                                       })
                                       .Returns(Task.CompletedTask);

            updateMonthPatternDto.Date = new DateTime(2025, 2, 1);
            updateMonthPatternDto.PatternId = 2;

            //act
            await _monthPatternService.UpdateMonthPatternAsync(updateMonthPatternDto);

            //assert
            caught.Should().NotBeNull();
            caught.Id.Should().Be(updateMonthPatternDto.Id);
            caught.UserId.Should().Be(userId);
            caught.Date.Should().Be(new DateTime(2025, 2, 1));
            caught.PatternId.Should().Be(2);
        }

        [Fact]
        public async Task DeleteMonthPatternAsync_ShouldThrowMonthPatternNotFoundException_WhenIdIsValidAndUserIdIsInValid()
        {
            //arrange
            var validId = 1;
            var invalidUserId = Guid.NewGuid();
            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(validId, invalidUserId))
                                       .ReturnsAsync((MonthPattern)null);

            //act & assert
            await _monthPatternService
                .Invoking(async service => await service.DeleteMonthPatternAsync(validId, invalidUserId))
                .Should()
                .ThrowAsync<MonthPatternNotFoundException>()
                .WithMessage($"MonthPattern not found exception. Id: {1}.");
        }

        [Fact]
        public async Task DeleteMonthPatternAsync_ShouldThrowMonthPatternNotFoundException_WhenIdIsInValidAndUserIdIsValid()
        {
            //arrange
            var invalidId = 2;
            var validUserId = Guid.NewGuid();

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(invalidId, validUserId))
                                       .ReturnsAsync((MonthPattern)null);

            //act & assert
            await _monthPatternService
                .Invoking(async service => await service.DeleteMonthPatternAsync(invalidId, validUserId))
                .Should()
                .ThrowAsync<MonthPatternNotFoundException>()
                .WithMessage($"MonthPattern not found exception. Id: {2}.");
        }

        [Fact]
        public async Task DeleteMonthPatternAsync_ShouldDeleteMonthPattern_WhenDataIsValid()
        {
            // arrange
            var userId = Guid.NewGuid();
            var id = 1;
            var existingMonthPattern = new MonthPattern { Id = id, UserId = userId, Date = new DateTime(2025, 1, 1), PatternId = 1 };

            _monthPatternRepositoryMock.SetupSequence(repo => repo.GetAsync(id, userId))
                                       .ReturnsAsync(existingMonthPattern)
                                       .ReturnsAsync((MonthPattern)null);
            _monthPatternRepositoryMock.Setup(repo => repo.DeleteAsync(existingMonthPattern))
                                       .Returns(Task.CompletedTask);
            // act
            await _monthPatternService.DeleteMonthPatternAsync(id, userId);

            // assert
            var result = await _monthPatternRepositoryMock.Object.GetAsync(id, userId);
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteMonthPatternAsync_ShouldCallRepositoryTwice_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            var id = 1;
            var existingMonthPattern = new MonthPattern { Id = id, UserId = userId, Date = new DateTime(2025, 1, 1), PatternId = 1 };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(id, userId))
                                       .ReturnsAsync(existingMonthPattern);
            _monthPatternRepositoryMock.Setup(repo => repo.DeleteAsync(existingMonthPattern))
                                       .Returns(Task.CompletedTask);

            //act
            await _monthPatternService.DeleteMonthPatternAsync(id, userId);

            //assert
            _monthPatternRepositoryMock.Verify(repo => repo.GetAsync(id, userId), Times.Once);
            _monthPatternRepositoryMock.Verify(repo => repo.DeleteAsync(existingMonthPattern), Times.Once);
        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldReturnPatternDtoWithValueMinusOne_WhenMonthPatternNotFound()
        {
            //arrange
            var userId = Guid.NewGuid();
            var month = 1;
            var year = 2025;

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<MonthYearModel>(), userId))
                                       .ReturnsAsync((MonthPattern)null);

            //act
            var result = await _monthPatternService.RetrieveMonthPatternAsync(month, year, userId);

            //assert
            result.Should().NotBeNull();
            result.Id.Should().Be(-1);
        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldThrowPatternNotFoundException_WhenPatternNotFound()
        {
            //arrange
            var userId = Guid.NewGuid();
            var month = 1;
            var year = 2025;
            var monthPattern = new MonthPattern { PatternId = 2, Date = new DateTime(year, month, 1), UserId = userId };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<MonthYearModel>(), userId))
                                       .ReturnsAsync(monthPattern);
            _patternRepositoryMock.Setup(repo => repo.GetAsync(monthPattern.PatternId, userId))
                                  .ReturnsAsync((Pattern)null);

            //act & assert
            await _monthPatternService
                .Invoking(async service => await service.RetrieveMonthPatternAsync(month, year, userId))
                .Should()
                .ThrowAsync<PatternNotFoundException>()
                .WithMessage($"Pattern not found. Id:{2}.");
        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldReturnPatternDto_WhenDataIsValid()
        {
            // arrange
            var userId = Guid.NewGuid();
            var month = 1;
            var year = 2025;
            var monthPattern = new MonthPattern { PatternId = 1, Date = new DateTime(year, month, 1), UserId = userId };
            var pattern = new Pattern { Id = 1, UserId = userId, Name = "Test Pattern", Value_Saves = 100, Value_Fees = 50, Value_Entertainment = 30 };
            var patternDto = new PatternDto { Id = 1, UserId = userId, Name = "Test Pattern", Value_Saves = 100, Value_Fees = 50, Value_Entertainment = 30 };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<MonthYearModel>(), userId))
                                       .ReturnsAsync(monthPattern);
            _patternRepositoryMock.Setup(repo => repo.GetAsync(pattern.Id, userId))
                                  .ReturnsAsync(pattern);
            _patternMapperMock.Setup(mapper => mapper.Map(pattern))
                              .Returns(patternDto);

            //act
            var result = await _monthPatternService.RetrieveMonthPatternAsync(month, year, userId);

            //assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Test Pattern");
            result.Value_Saves.Should().Be(100);
            result.Value_Fees.Should().Be(50);
            result.Value_Entertainment.Should().Be(30);
        }

        [Fact]
        public async Task RetrieveMonthPatternAsync_ShouldCallRepositoryTwiceAndCallMapperOnce()
        {
            // arrange
            var userId = Guid.NewGuid();
            var month = 1;
            var year = 2025;
            var monthPattern = new MonthPattern { PatternId = 1, Date = new DateTime(year, month, 1), UserId = userId };
            var pattern = new Pattern { Id = 1, UserId = userId, Name = "Test Pattern", Value_Saves = 100, Value_Fees = 50, Value_Entertainment = 30 };
            var patternDto = new PatternDto { Id = 1, UserId = userId, Name = "Test Pattern", Value_Saves = 100, Value_Fees = 50, Value_Entertainment = 30 };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<MonthYearModel>(), userId))
                                       .ReturnsAsync(monthPattern);
            _patternRepositoryMock.Setup(repo => repo.GetAsync(pattern.Id, userId))
                                  .ReturnsAsync(pattern);
            _patternMapperMock.Setup(mapper => mapper.Map(pattern))
                              .Returns(patternDto);

            //act
            var result = await _monthPatternService.RetrieveMonthPatternAsync(month, year, userId);

            //assert
            _monthPatternRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<MonthYearModel>(), userId), Times.Once);
            _patternRepositoryMock.Verify(repo => repo.GetAsync(pattern.Id, userId), Times.Once);
            _patternMapperMock.Verify(mapper => mapper.Map(It.IsAny<Pattern>()), Times.Once);
        }

        [Fact]
        public async Task RetrievePatternsAsync_ShouldReturnFullMonthPatternDtoList_WhenDataIsValid()
        {
            //arrange
            var userId = Guid.NewGuid();
            var monthPatterns = new List<MonthPattern>
            {
                new MonthPattern { Id = 1, UserId = userId, Date = new DateTime(2025, 1, 1), PatternId = 1 },
                new MonthPattern { Id = 2, UserId = userId, Date = new DateTime(2025, 2, 1), PatternId = 2 }
            };
            var patterns = new List<Pattern>
            {
                new Pattern { Id = 1, UserId = userId, Name = "Pattern 1", Value_Saves = 100, Value_Fees = 50, Value_Entertainment = 30 },
                new Pattern { Id = 2, UserId = userId, Name = "Pattern 2", Value_Saves = 200, Value_Fees = 100, Value_Entertainment = 60 }
            };

            _monthPatternRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                                       .ReturnsAsync(monthPatterns);
            _patternRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                                  .ReturnsAsync(patterns);

            //act
            var result = await _monthPatternService.RetrievePatternsAsync(userId);

            //assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            var resultList = result.ToList();
            resultList[0].Pattern.Name.Should().Be("Pattern 2");
            resultList[1].Pattern.Name.Should().Be("Pattern 1");
        }

        [Fact]
        public async Task RetrievePatternsAsync_ShouldReturnEmptyList_WhenUserIdIsInValid()
        {    
            //arrange
            var invalidUserId = Guid.NewGuid();
            _monthPatternRepositoryMock.Setup(repo => repo.GetAllAsync(invalidUserId))
                                       .ReturnsAsync(new List<MonthPattern>());
            _patternRepositoryMock.Setup(repo => repo.GetAllAsync(invalidUserId))
                                  .ReturnsAsync(new List<Pattern>());

            //act
            var result = await _monthPatternService.RetrievePatternsAsync(invalidUserId);

            //assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task RetrievePatternsAsync_ShouldCallRepositoryTwice_WhenCalled()
        {
            //arrange
            var userId = Guid.NewGuid();
            _monthPatternRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                                       .ReturnsAsync(new List<MonthPattern>());
            _patternRepositoryMock.Setup(repo => repo.GetAllAsync(userId))
                                  .ReturnsAsync(new List<Pattern>());

            //act
            await _monthPatternService.RetrievePatternsAsync(userId);

            //assert
            _monthPatternRepositoryMock.Verify(repo => repo.GetAllAsync(userId), Times.Once);
            _patternRepositoryMock.Verify(repo => repo.GetAllAsync(userId), Times.Once);
        }
    }
}