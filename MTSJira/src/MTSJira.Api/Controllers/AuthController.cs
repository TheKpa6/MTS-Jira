using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTSJira.Api.Models.Auth;
using MTSJira.Application.Services.JwtService.Contracts;
using System.IdentityModel.Tokens.Jwt;
using LoginRequest = MTSJira.Api.Models.Auth.LoginRequest;

namespace MTSJira.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest model)
        {
            var token = _jwtService.GetJwtSecurityToken(model.Login);

            return Ok(new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
