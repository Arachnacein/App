using FluentAssertions;
using IdentityManager.Exceptions;
using IdentityManager.Models;
using IdentityManager.Models.Enums;
using IdentityManager.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ServicesTests.cs
{
    public class AccountServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);
            _accountService = new AccountService(_userManagerMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowCustomException_WhenUsernameAlreadyExists()
        {
            // Arrange
            var existingUser = new ApplicationUser { UserName = "existingUser" };
            _userManagerMock.Setup(x => x.FindByNameAsync("existingUser")).ReturnsAsync(existingUser);

            var model = new RegistrationModel { Username = "existingUser", Email = "new@example.com", Password = "Pass@word1" };

            // Act
            Func<Task> act = async () => await _accountService.RegisterAsync(model);

            // Assert
            await act.Should().ThrowAsync<CustomException>()
                .Where(e => e.ErrorCode == (int)ErrorCodesEnum.UsernameAlreadyExists);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowCustomException_WhenEmailAlreadyExists()
        {
            // Arrange
            var existingUser = new ApplicationUser { Email = "existing@example.com" };
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);
            _userManagerMock.Setup(x => x.FindByEmailAsync("existing@example.com")).ReturnsAsync(existingUser);

            var model = new RegistrationModel { Username = "newuser", Email = "existing@example.com", Password = "Pass@word1" };

            // Act
            Func<Task> act = async () => await _accountService.RegisterAsync(model);

            // Assert
            await act.Should().ThrowAsync<CustomException>()
                .Where(e => e.ErrorCode == (int)ErrorCodesEnum.EmailAlreadyExists);
        }

        [Fact]
        public async Task RegisterAsync_ShouldSucceed_WhenUserIsNew()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var model = new RegistrationModel
            {
                Username = "newuser",
                Email = "new@example.com",
                Password = "Pass@word1",
                FirstName = "Jan",
                LastName = "Kowalski"
            };

            // Act
            Func<Task> act = async () => await _accountService.RegisterAsync(model);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowCustomException_WhenIdentityCreateFails()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password too weak." }));

            var model = new RegistrationModel { Username = "user", Email = "user@example.com", Password = "weak" };

            // Act
            Func<Task> act = async () => await _accountService.RegisterAsync(model);

            // Assert
            await act.Should().ThrowAsync<CustomException>()
                .Where(e => e.ErrorCode == (int)ErrorCodesEnum.RegistrationFailed);
        }
    }
}
