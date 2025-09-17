using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTSJira.Api.Models;
using MTSJira.Application.Services.JwtService.Contracts;
using MTSJira.Domain.Common.Enums;
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

        /// <summary>
        /// Метод для авторизации пользователей
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <returns>JWT токен</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResult<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResultNoData))]
        public ActionResult<ApiResult<string>> Login([FromBody] LoginRequest request)
        {
            if (request.Password != "qwerty123")
                return BadRequest(new ApiResultNoData("The password is incorrect. Use password: 'qwerty123'"));

            var result = _jwtService.GetJwtSecurityToken(request.Login);

            switch (result.ErrorCode)
            {
                case CommonErrorCode.None:
                    return Ok(new ApiResult<string>(new JwtSecurityTokenHandler().WriteToken(result.Data)));
                case CommonErrorCode.Exception:
                    return BadRequest(new ApiResultNoData(result.Message, result.Ex.StackTrace));
                default:
                    return BadRequest(new ApiResultNoData(result.Message));
            }
        }
    }
}
