namespace IdentityManager.Services;

public class AccountService : IAccountService
{
    private const string DefaultRole = "user";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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

        if (!await _roleManager.RoleExistsAsync(DefaultRole))
            await _roleManager.CreateAsync(new IdentityRole(DefaultRole));

        await _userManager.AddToRoleAsync(user, DefaultRole);
    }
}

public interface IAccountService
{
    Task RegisterAsync(RegistrationModel model, CancellationToken ct = default);
}