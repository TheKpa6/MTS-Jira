using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MTSJira.Application.Handlers;
using MTSJira.Application.Services.JwtService.Contracts;
using MTSJira.Application.Services.JwtService.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MTSJira.Application.Services.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly ILogger<JwtService> _logger;
        private readonly JwtAuthOptions _jwtAuthOptions;

        public JwtService(ILogger<JwtService> logger, IOptions<JwtAuthOptions> jwtAuthOptions)
        {
            _logger = logger;
            _jwtAuthOptions = jwtAuthOptions.Value;
        }

        public ApplicationCommonServiceHandlerResult<JwtSecurityToken> GetJwtSecurityToken(string login)
        {
            try
            {
                _logger.LogInformation("{MethodName}: Get JWT security token for login: {Login}", nameof(GetJwtSecurityToken), login);

                var identity = GetIdentity(login);

                var time = _jwtAuthOptions.Lifetime;
                #if DEBUG
                time = 86400;
                #endif

                var now = DateTime.UtcNow;
                return ApplicationCommonServiceHandlerResult<JwtSecurityToken>.CreateSuccess(new JwtSecurityToken(
                       issuer: _jwtAuthOptions.Issuer,
                       audience: _jwtAuthOptions.Audience,
                       notBefore: now,
                       claims: identity.Claims,
                       expires: now.Add(TimeSpan.FromSeconds(time)),
                       signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthOptions.Key)), SecurityAlgorithms.HmacSha512)));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Can't create JWT security token for login: {Login}", login);
                return ApplicationCommonServiceHandlerResult<JwtSecurityToken>.CreateException(ex);
            }
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
