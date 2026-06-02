using FluentAssertions;
using IdentityManager.Models;
using IdentityManager.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ServicesTests.cs
{
    public class JwtServiceTests
    {
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public JwtServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "TestSecretKey-ForUnitTests-MinLength32!" },
                { "Jwt:Issuer", "http://test-issuer" },
                { "Jwt:Audience", "test-audience" },
                { "Jwt:ExpiryMinutes", "60" },
                { "Jwt:RefreshTokenExpiryDays", "7" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            _jwtService = new JwtService(_configuration);
        }

        [Fact]
        public void GenerateAccessToken_ShouldReturnNonEmptyToken()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testuser",
                Email = "test@example.com",
                FirstName = "Jan",
                LastName = "Kowalski",
                EmailConfirmed = false,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var token = _jwtService.GenerateAccessToken(user, new List<string> { "user" });

            // Assert
            token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GenerateAccessToken_ShouldContainExpectedClaims()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = new ApplicationUser
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@example.com",
                FirstName = "Jan",
                LastName = "Kowalski",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var token = _jwtService.GenerateAccessToken(user, new List<string> { "admin" });

            // Assert
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            jwtToken.Claims.Should().Contain(c => c.Type == "userId" && c.Value == userId);
            jwtToken.Claims.Should().Contain(c => c.Type == "given_name" && c.Value == "Jan");
            jwtToken.Claims.Should().Contain(c => c.Type == "family_name" && c.Value == "Kowalski");
            jwtToken.Claims.Should().Contain(c => c.Type == "role" && c.Value == "admin");
        }

        [Fact]
        public void GenerateRefreshToken_ShouldReturnValidToken()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            // Act
            var refreshToken = _jwtService.GenerateRefreshToken(userId);

            // Assert
            refreshToken.Should().NotBeNull();
            refreshToken.Token.Should().NotBeNullOrEmpty();
            refreshToken.UserId.Should().Be(userId);
            refreshToken.IsRevoked.Should().BeFalse();
            refreshToken.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public void GenerateRefreshToken_ShouldGenerateUniqueTokens()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            // Act
            var token1 = _jwtService.GenerateRefreshToken(userId);
            var token2 = _jwtService.GenerateRefreshToken(userId);

            // Assert
            token1.Token.Should().NotBe(token2.Token);
        }
    }
}
