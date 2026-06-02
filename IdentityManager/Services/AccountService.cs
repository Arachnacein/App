using IdentityManager.Exceptions;
using IdentityManager.Models;
using IdentityManager.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace IdentityManager.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task RegisterAsync(RegistrationModel model, CancellationToken ct = default)
        {
            if (await _userManager.FindByNameAsync(model.Username) != null)
                throw new CustomException((int)ErrorCodesEnum.UsernameAlreadyExists, "Username already exists.");

            if (await _userManager.FindByEmailAsync(model.Email) != null)
                throw new CustomException((int)ErrorCodesEnum.EmailAlreadyExists, "Email already exists.");

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                throw new CustomException((int)ErrorCodesEnum.RegistrationFailed,
                    string.Join("; ", result.Errors.Select(e => e.Description)));
        }
    }

    public interface IAccountService
    {
        Task RegisterAsync(RegistrationModel model, CancellationToken ct = default);
    }
}
