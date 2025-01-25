
using FluentAssertions;
using IdentityManager.Models;
using IdentityManager.Services;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace ServicesTests.cs
{
    public class UserServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly HttpClient _httpClient;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _tokenServiceMock = new Mock<ITokenService>();
            _userService = new UserService(_httpClient, _tokenServiceMock.Object);
        }

        [Fact]
        public async Task UpdateUserPropertiesAsync_ShouldReturnTrue_WhenResponseIsSuccessful()
        {
            // Arrange
            var userModel = new UserModel
            {
                UserId = Guid.NewGuid(),
                Username = "testuser",
                Email = "testuser@example.com",
                FirstName = "Test",
                LastName = "User"
            };
            _tokenServiceMock.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("valid_token");
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Act
            var result = await _userService.UpdateUserPropertiesAsync(userModel);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetUserDataAsync_ShouldReturnUserModel_WhenResponseIsSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userModel = new UserModel
            {
                UserId = userId,
                Username = "testuser",
                Email = "testuser@example.com",
                FirstName = "Test",
                LastName = "User"
            };
            _tokenServiceMock.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("valid_token");
            var jsonResponse = JsonSerializer.Serialize(new
            {
                id = userModel.UserId,
                username = userModel.Username,
                email = userModel.Email,
                firstName = userModel.FirstName,
                lastName = userModel.LastName
            });
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            // Act
            var result = await _userService.GetUserDataAsync(userId);

            // Assert
            result.Should().BeEquivalentTo(userModel, options => options.Excluding(x => x.UserId));
        }

        [Fact]
        public async Task GetUserDataAsync_ShouldThrowException_WhenUserIsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _tokenServiceMock.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("valid_token");
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("null")
                });

            // Act
            Func<Task> act = async () => await _userService.GetUserDataAsync(userId);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage($"User not found. ID: {userId}");
        }

        [Fact]
        public async Task GetUserDataAsync_ShouldThrowException_WhenAdminTokenIsNullOrEmpty()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _tokenServiceMock.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync(string.Empty);

            // Act
            Func<Task> act = async () => await _userService.GetUserDataAsync(userId);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Failed to retrieve admin token.");
        }

        [Fact]
        public async Task SendVerificationEmailAsync_ShouldThrowException_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _tokenServiceMock.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("valid_token");
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            // Act
            Func<Task> act = async () => await _userService.SendVerificationEmailAsync(userId);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Error while sending email verification request to keycloak.");
        }

        [Fact]
        public async Task SendVerificationEmailAsync_ShouldThrowException_WhenAdminTokenIsNullOrEmpty()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _tokenServiceMock.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync(string.Empty);

            // Act
            Func<Task> act = async () => await _userService.SendVerificationEmailAsync(userId);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Failed to retrieve admin token.");
        }
    }
}