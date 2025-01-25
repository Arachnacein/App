using IdentityManager.Services;
using Moq.Protected;
using Moq;
using System.Net;
using FluentAssertions;

namespace ServicesTests.cs
{
    public class LoginServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly LoginService _loginService;

        public LoginServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _loginService = new LoginService(_httpClient);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnAccessToken_WhenResponseIsSuccessful()
        {
            // Arrange
            var expectedToken = "test_access_token";
            var responseContent = $"{{\"access_token\": \"{expectedToken}\"}}";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent)
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _loginService.LoginAsync("username", "password");

            // Assert
            result.Should().Be(expectedToken);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _loginService.LoginAsync("username", "password");

            // Assert
            result.Should().BeNull();
        }
    }
}