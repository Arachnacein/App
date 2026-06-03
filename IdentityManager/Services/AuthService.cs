using IdentityManager.Data;
using IdentityManager.Exceptions;
using IdentityManager.Models;
using IdentityManager.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityManager.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly ApplicationDbContext _context;

    public AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService, ApplicationDbContext context)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _context = context;
    }

    public async Task<(string accessToken, string refreshToken)> LoginAsync(string username, string password, CancellationToken ct = default)
    {
        var user = await _userManager.FindByNameAsync(username)
                   ?? await _userManager.FindByEmailAsync(username);

        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            throw new CustomException((int)ErrorCodesEnum.InvalidCredentials, "Invalid username or password.");

        if (!user.IsActive)
            throw new CustomException((int)ErrorCodesEnum.UserNotActive, "Account is disabled.");

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtService.GenerateAccessToken(user, roles);
        var refreshToken = _jwtService.GenerateRefreshToken(user.Id);

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync(ct);

        return (accessToken, refreshToken.Token);
    }

    public async Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string token, CancellationToken ct = default)
    {
        var existingToken = await _context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == token, ct);

        if (existingToken == null || existingToken.IsRevoked || existingToken.ExpiresAt < DateTime.UtcNow)
            throw new CustomException((int)ErrorCodesEnum.InvalidRefreshToken, "Invalid or expired refresh token.");

        existingToken.IsRevoked = true;

        var user = existingToken.User;
        var roles = await _userManager.GetRolesAsync(user);
        var newAccessToken = _jwtService.GenerateAccessToken(user, roles);
        var newRefreshToken = _jwtService.GenerateRefreshToken(user.Id);

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync(ct);

        return (newAccessToken, newRefreshToken.Token);
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken ct = default)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshToken, ct);

        if (token != null)
        {
            token.IsRevoked = true;
            await _context.SaveChangesAsync(ct);
        }
    }
}

public interface IAuthService
{
    Task<(string accessToken, string refreshToken)> LoginAsync(string username, string password, CancellationToken ct = default);
    Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string token, CancellationToken ct = default);
    Task LogoutAsync(string refreshToken, CancellationToken ct = default);
}
