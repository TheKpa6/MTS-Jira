using Microsoft.IdentityModel.Tokens;
using MTSJira.Application.Services.JwtService.Contracts;
using MTSJira.Application.Services.JwtService.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MTSJira.Application.Services.JwtService
{
    public class JwtService : IJwtService
    {
        public JwtSecurityToken GetJwtSecurityToken(string login)
        {
            var identity = GetIdentity(login);

            var time = JwtAuthOptions.LIFETIME;
#if DEBUG
            time = 86400;
#endif

            var now = DateTime.UtcNow;
            return new JwtSecurityToken(
                   issuer: JwtAuthOptions.ISSUER,
                   audience: JwtAuthOptions.AUDIENCE,
                   notBefore: now,
                   claims: identity.Claims,
                   expires: now.Add(TimeSpan.FromSeconds(time)),
                   signingCredentials: new SigningCredentials(JwtAuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha512));
        }

        private ClaimsIdentity GetIdentity(string login)
        {
            var claims = new List<Claim>
                {
                    new Claim("Login", login ?? string.Empty),
                };

            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimTypes.Role);
        }
    }
}
