using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;

namespace MTSJira.Application.Services.JwtService.Contracts
{
    public interface IJwtService
    {
        JwtSecurityToken GetJwtSecurityToken(string login);
    }
}
