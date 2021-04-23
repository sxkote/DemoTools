using DemoTools.Authorization.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SX.Common.Shared.Contracts;

namespace DemoTools.Authorization.Api.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationProvider _authenticationProvider;

        public AuthenticationController(IAuthenticationProvider authenticationProvider)
        {
            _authenticationProvider = authenticationProvider;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] LoginModel dto)
        {
            var token = _authenticationProvider.Authenticate(dto.Login, dto.Password);

            if (token == null || !token.IsValid())
                return Unauthorized();

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("token/{code}")]
        public IActionResult GetToken(string code)
        {
            var token = _authenticationProvider.Authenticate(code);

            if (token == null || !token.IsValid())
                return Unauthorized();

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new LoginModel { Login = "guest", Password = "123456" });
        }
    }
}
