using MTSJira.Application.Handlers;
using System.IdentityModel.Tokens.Jwt;

namespace MTSJira.Application.Services.JwtService.Contracts
{
    public interface IJwtService
    {
        ApplicationCommonServiceHandlerResult<JwtSecurityToken> GetJwtSecurityToken(string login);
    }
}
