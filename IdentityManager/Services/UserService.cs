using IdentityManager.Exceptions;
using IdentityManager.Models;
using IdentityManager.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace IdentityManager.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserModel> GetUserDataAsync(Guid userId, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new CustomException((int)ErrorCodesEnum.UserNotFound, $"User not found. ID: {userId}");

            var roles = await _userManager.GetRolesAsync(user);
            return MapToUserModel(user, roles);
        }

        public async Task<List<UserModel>> GetUsersAsync(CancellationToken ct = default)
        {
            var users = _userManager.Users.ToList();
            var result = new List<UserModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(MapToUserModel(user, roles));
            }
            return result;
        }

        public async Task UpdateUserPropertiesAsync(UserModel model, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
                throw new CustomException((int)ErrorCodesEnum.UserNotFound, $"User not found. ID: {model.UserId}");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.Username;
            user.Email = model.Email;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new CustomException((int)ErrorCodesEnum.GeneralError,
                    string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        public async Task EnableDisableUserAsync(UserModel model, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
                throw new CustomException((int)ErrorCodesEnum.UserNotFound, $"User not found. ID: {model.UserId}");

            user.IsActive = model.Enabled;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        public async Task SendVerificationEmailAsync(Guid userId, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new CustomException((int)ErrorCodesEnum.UserNotFound, $"User not found. ID: {userId}");

            // Generates confirmation token - wire up IEmailSender to send the actual email
            _ = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task ChangePasswordAsync(ChangePasswordModel model, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
                throw new CustomException((int)ErrorCodesEnum.UserNotFound, $"User not found. ID: {model.UserId}");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
                throw new CustomException((int)ErrorCodesEnum.GeneralError,
                    string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        public async Task AssignRoleAsync(Guid userId, string role, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new CustomException((int)ErrorCodesEnum.UserNotFound, $"User not found. ID: {userId}");

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task RemoveRoleAsync(Guid userId, string role, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new CustomException((int)ErrorCodesEnum.UserNotFound, $"User not found. ID: {userId}");

            await _userManager.RemoveFromRoleAsync(user, role);
        }

        private static UserModel MapToUserModel(ApplicationUser user, IList<string> roles) => new()
        {
            UserId = Guid.Parse(user.Id),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            AccountCreatedDate = user.CreatedAt,
            Roles = roles.ToList(),
            Enabled = user.IsActive,
            EmailVerified = user.EmailConfirmed
        };
    }

    public interface IUserService
    {
        Task<UserModel> GetUserDataAsync(Guid userId, CancellationToken ct = default);
        Task<List<UserModel>> GetUsersAsync(CancellationToken ct = default);
        Task UpdateUserPropertiesAsync(UserModel model, CancellationToken ct = default);
        Task EnableDisableUserAsync(UserModel model, CancellationToken ct = default);
        Task SendVerificationEmailAsync(Guid userId, CancellationToken ct = default);
        Task ChangePasswordAsync(ChangePasswordModel model, CancellationToken ct = default);
        Task AssignRoleAsync(Guid userId, string role, CancellationToken ct = default);
        Task RemoveRoleAsync(Guid userId, string role, CancellationToken ct = default);
    }
}
