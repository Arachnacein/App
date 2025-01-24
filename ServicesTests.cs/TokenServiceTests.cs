using FluentAssertions;
using IdentityManager.Exceptions;
using IdentityManager.Models.Enums;
using IdentityManager.Services;
using Moq;
using Moq.Protected;
using System.Net;

namespace ServicesTests.cs
{
    public class TokenServiceTests //by copilot
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _tokenService = new TokenService(_httpClient);
        }

        [Fact]
        public async Task GetAdminTokenAsync_ShouldReturnToken_WhenResponseIsSuccessful()
        {
            // Arrange
            var responseContent = "{\"access_token\": \"test_token\"}";
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
            var token = await _tokenService.GetAdminTokenAsync();

            // Assert
            token.Should().Be("test_token");
        }

        [Fact]
        public async Task GetAdminTokenAsync_ShouldThrowCustomException_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Error")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            Func<Task> act = async () => await _tokenService.GetAdminTokenAsync();

            // Assert
            await act.Should().ThrowAsync<CustomException>()
                .WithMessage("Failed to fetch admin token.")
                .Where(e => e.ErrorCode == (int)ErrorCodesEnum.AdminTokenFetchFailed);
        }
    }
}