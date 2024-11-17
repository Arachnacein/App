using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace UI.Extensions
{
    public static class TokenExtensions
    {
        public static List<string> GetUserRolesFromToken(this JwtSecurityToken token)
        {

            var roles = token.Claims.Where(x => x.Type == "realm_access")
                                    .SelectMany(x => JsonDocument.Parse(x.Value).RootElement.GetProperty("roles").EnumerateArray())
                                    .Select(role => role.GetString())
                                    .ToList();

            return roles;
        }
    }
}