using BudgetManager.Exceptions;
using BudgetManager.Exceptions.IncomeExceptions;
using BudgetManager.Exceptions.PatternExceptions;
using BudgetManager.Exceptions.TransactionExceptions;
using BudgetManager.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace MiddlewaresTests
{
    public class ExceptionHandlingtTests
    {
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly Mock<ILogger<ExceptionHandling>> _mockLogger;
        private readonly ExceptionHandling _exceptionHandling;

        public ExceptionHandlingtTests()
        {
            _mockNext = new Mock<RequestDelegate>();
            _mockLogger = new Mock<ILogger<ExceptionHandling>>();
            _exceptionHandling = new ExceptionHandling(_mockNext.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task InvokeAsync_ShouldCallContext_WhenDataIsValid()
        {
            //arrange
            var context = new DefaultHttpContext();

            _mockNext.Setup(next => next(context))
                     .Returns(Task.CompletedTask);
            //act
            await _exceptionHandling.InvokeAsync(context);

            //assert
            _mockNext.Verify(next => next(context), Times.Once);

        }

        [Fact]
        public async Task InvokeAsync_ShouldCallHandleException_WhenDataIsInValid()
        {
            //arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            var exception = new NullPointerException("NullPointerException");

            _mockNext.Setup(next => next(context))
                     .ThrowsAsync(exception);

            //act
            await _exceptionHandling.InvokeAsync(context);

            //assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();

            Assert.Equal(StatusCodes.Status409Conflict, context.Response.StatusCode);
            Assert.False(string.IsNullOrWhiteSpace(responseText), "Response body is empty");
        }

        [Theory]
        [InlineData(typeof(NullPointerException), 409, "Object is Null.")]
        [InlineData(typeof(BadStringLengthException), 409, "Invalid string length.")]
        [InlineData(typeof(BadValueException), 409, "Invalid value provided.")]
        [InlineData(typeof(TransactionNotFoundException), 409, "Transaction not found.")]
        [InlineData(typeof(PatternNotFoundException), 409, "Pattern not found.")]
        [InlineData(typeof(IncomeNotFoundException), 409, "Income not found.")]
        [InlineData(typeof(MonthPatternAlreadyExistsException), 409, "Month pattern already exists.")]
        public async Task HandleExceptionAsync_ShouldLogErrorWithMesageAndExceptionType_WhenCalled(Type exceptionType, int expectedStatusCode, string expectedMessage)
        {
            //arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            Exception exception = exceptionType switch
            {
                Type t when t == typeof(NullPointerException) => new NullPointerException(expectedMessage),
                Type t when t == typeof(BadStringLengthException) => new BadStringLengthException(expectedMessage),
                Type t when t == typeof(BadValueException) => new BadValueException(expectedMessage),
                Type t when t == typeof(TransactionNotFoundException) => new TransactionNotFoundException(expectedMessage),
                Type t when t == typeof(PatternNotFoundException) => new PatternNotFoundException(expectedMessage),
                Type t when t == typeof(IncomeNotFoundException) => new IncomeNotFoundException(expectedMessage),
                Type t when t == typeof(MonthPatternAlreadyExistsException) => new MonthPatternAlreadyExistsException(expectedMessage),
                _ => throw new ArgumentException($"Unhandled exception type: {exceptionType}")
            };

            _mockNext
                .Setup(next => next(context))
                .ThrowsAsync(exception);

            //act
            await _exceptionHandling.InvokeAsync(context);

            //assert

            var responseBody = context.Response.Body;
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();
            var responseJson = JsonSerializer.Deserialize<JsonElement>(responseText);

            Assert.Equal(expectedStatusCode, context.Response.StatusCode);
            Assert.Equal(expectedMessage, responseJson.GetProperty("error").GetString());
        }
        
        [Fact]
        public async Task HandleExceptionAsync_LogsError_WhenExceptionThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            var exception = new NullPointerException("");

            _mockNext.Setup(next => next(context)).ThrowsAsync(exception);

            // Act
            await _exceptionHandling.InvokeAsync(context);

            // Assert
            _mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Error,        
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Object is Null.")),
                    exception,
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Once
            );
        }
    }
}