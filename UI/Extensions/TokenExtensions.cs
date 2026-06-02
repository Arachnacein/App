using System.IdentityModel.Tokens.Jwt;

namespace UI.Extensions
{
    public static class TokenExtensions
    {
        public static List<string> GetUserRolesFromToken(this JwtSecurityToken token)
        {
            return token.Claims
                .Where(x => x.Type == "role")
                .Select(x => x.Value)
                .ToList()!;
        }
    }
}
