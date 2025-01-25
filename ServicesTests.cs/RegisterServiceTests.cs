using IdentityManager.Exceptions;
using IdentityManager.Models;
using IdentityManager.Services;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ServicesTests.cs
{
    public class RegisterServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly RegisterService _registerService;
        private readonly HttpClient _httpClient;

        public RegisterServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _tokenServiceMock = new Mock<ITokenService>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _registerService = new RegisterService(_httpClient, _tokenServiceMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFalse_WhenAdminTokenIsEmpty()
        {
            // Arrange
            _tokenServiceMock.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync(string.Empty);
            var model = new RegistrationModel();

            // Act
            var result = await _registerService.RegisterAsync(model);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenUsernameExists()
        {
            // Arrange
            _tokenServiceMock.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("adminToken");
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(new List<object> { new { } }), Encoding.UTF8, "application/json")
                });
            var model = new RegistrationModel { Username = "existingUser" };

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _registerService.RegisterAsync(model));
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenEmailExists()
        {
            // Arrange
            _tokenServiceMock.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("adminToken");
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(new List<object> { new { } }), Encoding.UTF8, "application/json")
                });
            var model = new RegistrationModel { Email = "existingEmail@example.com" };

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _registerService.RegisterAsync(model));
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnTrue_WhenRegistrationIsSuccessful()
        {
            // Arrange
            _tokenServiceMock.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("adminToken");
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent(JsonSerializer.Serialize(new List<object>()), Encoding.UTF8, "application/json")
                });
            var model = new RegistrationModel { Username = "newUser", Email = "newEmail@example.com", Password = "password" };

            // Act
            var result = await _registerService.RegisterAsync(model);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UsernameExistsAsync_ShouldReturnFalse_WhenAdminTokenIsEmpty()
        {
            // Arrange
            _tokenServiceMock.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync(string.Empty);

            // Act
            var result = await _registerService.UsernameExistsAsync("username");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UsernameExistsAsync_ShouldReturnTrue_WhenUsernameExists()
        {
            // Arrange
            _tokenServiceMock.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("adminToken");
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new List<object> { new { } }), Encoding.UTF8, "application/json")
            };
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _registerService.UsernameExistsAsync("existingUser");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EmailExistsAsync_ShouldReturnFalse_WhenAdminTokenIsEmpty()
        {
            // Arrange
            _tokenServiceMock.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync(string.Empty);

            // Act
            var result = await _registerService.EmailExistsAsync("email@example.com");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EmailExistsAsync_ShouldReturnTrue_WhenEmailExists()
        {
            // Arrange
            _tokenServiceMock.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("adminToken");
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new List<object> { new { } }), Encoding.UTF8, "application/json")
            };
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _registerService.EmailExistsAsync("existingEmail@example.com");

            // Assert
            Assert.True(result);
        }
    }
}