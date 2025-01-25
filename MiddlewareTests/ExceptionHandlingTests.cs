
using IdentityManager.Exceptions;
using IdentityManager.Middlewares;
using IdentityManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace MiddlewareTests
{
    public class ExceptionHandlingTests
    {
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly Mock<ILogger<ExceptionHandling>> _loggerMock;
        private readonly ExceptionHandling _middleware;

        public ExceptionHandlingTests()
        {
            _nextMock = new Mock<RequestDelegate>();
            _loggerMock = new Mock<ILogger<ExceptionHandling>>();
            _middleware = new ExceptionHandling(_nextMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task InvokeAsync_Should_Call_NextDelegate()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            await _middleware.Invoke(context);

            _nextMock.Verify(next => next(context), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_Should_Handle_CustomException()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            var customException = new CustomException(400, "Custom error");
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(customException);

            await _middleware.Invoke(context);

            _loggerMock.Verify(logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);

            Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var response = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(response);
            Assert.Equal(customException.ErrorCode, errorResponse.ErrorCode);
            Assert.Equal(customException.Message, errorResponse.Message);
        }

        [Fact]
        public async Task InvokeAsync_Should_Handle_GeneralException()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            var generalException = new Exception("General error");
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(generalException);

            await _middleware.Invoke(context);

            _loggerMock.Verify(logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);

            Assert.Equal(StatusCodes.Status409Conflict, context.Response.StatusCode);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var response = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(response);
            Assert.Equal(409, errorResponse.ErrorCode);
            Assert.Equal("Conflict", errorResponse.Message);
        }
    }
}
