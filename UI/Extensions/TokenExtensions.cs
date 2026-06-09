namespace UI.Extensions;

public static class TokenExtensions
{
    /// <summary>
    /// Extracts the user roles from a JWT security token by filtering the claims for those with the type "role" and returning their values as a list of strings.
    /// </summary>
    /// <param name="token">The JWT security token.</param>
    /// <returns>A list of user roles.</returns>
    public static List<string> GetUserRolesFromToken(this JwtSecurityToken token)
    {
        return token.Claims
            .Where(x => x.Type == "role")
            .Select(x => x.Value)
            .ToList()!;
    }
}