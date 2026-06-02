using FluentAssertions;
using IdentityManager.Exceptions;
using IdentityManager.Models;
using IdentityManager.Models.Enums;
using IdentityManager.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ServicesTests.cs
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStore.Object, null, null, null, null, null, null, null, null);

            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                roleStore.Object, null, null, null, null);

            _userService = new UserService(_userManagerMock.Object, _roleManagerMock.Object);
        }

        [Fact]
        public async Task GetUserDataAsync_ShouldReturnUserModel_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = userId.ToString(),
                UserName = "testuser",
                Email = "test@example.com",
                FirstName = "Jan",
                LastName = "Kowalski",
                IsActive = true,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };
            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "user" });

            // Act
            var result = await _userService.GetUserDataAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("testuser");
            result.Email.Should().Be("test@example.com");
            result.FirstName.Should().Be("Jan");
            result.LastName.Should().Be("Kowalski");
            result.Enabled.Should().BeTrue();
            result.EmailVerified.Should().BeTrue();
            result.Roles.Should().Contain("user");
        }

        [Fact]
        public async Task GetUserDataAsync_ShouldThrowCustomException_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString())).ReturnsAsync((ApplicationUser?)null);

            // Act
            Func<Task> act = async () => await _userService.GetUserDataAsync(userId);

            // Assert
            await act.Should().ThrowAsync<CustomException>()
                .Where(e => e.ErrorCode == (int)ErrorCodesEnum.UserNotFound);
        }

        [Fact]
        public async Task UpdateUserPropertiesAsync_ShouldSucceed_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser { Id = userId.ToString(), UserName = "old", Email = "old@example.com" };
            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            var model = new UserModel
            {
                UserId = userId,
                Username = "newuser",
                Email = "new@example.com",
                FirstName = "Nowe",
                LastName = "Nazwisko"
            };

            // Act
            Func<Task> act = async () => await _userService.UpdateUserPropertiesAsync(model);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task UpdateUserPropertiesAsync_ShouldThrowCustomException_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString())).ReturnsAsync((ApplicationUser?)null);

            var model = new UserModel { UserId = userId };

            // Act
            Func<Task> act = async () => await _userService.UpdateUserPropertiesAsync(model);

            // Assert
            await act.Should().ThrowAsync<CustomException>()
                .Where(e => e.ErrorCode == (int)ErrorCodesEnum.UserNotFound);
        }

        [Fact]
        public async Task EnableDisableUserAsync_ShouldUpdateIsActive_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser { Id = userId.ToString(), IsActive = true };
            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            var model = new UserModel { UserId = userId, Enabled = false };

            // Act
            await _userService.EnableDisableUserAsync(model);

            // Assert
            user.IsActive.Should().BeFalse();
        }
    }
}
