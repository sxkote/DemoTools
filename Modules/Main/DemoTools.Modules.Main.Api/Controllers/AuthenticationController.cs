using DemoTools.Modules.Main.Api.DTO.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SX.Common.Shared.Contracts;

namespace DemoTools.Modules.Main.Api.Controllers
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
        public IActionResult Authenticate([FromBody] LoginDTO dto)
        {
            var token = _authenticationProvider.Authenticate(dto.Login, dto.Password);

            if (token == null || !token.IsValid())
                return Unauthorized();

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new LoginDTO { Login = "sxkote", Password = "123456" });
        }
    }
}
