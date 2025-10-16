using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teslow_srv.Api.Services;
using Teslow_srv.Domain.Dto.Auth;
using Teslow_srv.Service.Interface;

namespace Teslow_srv.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var user = await _userService.ValidateCredentialsAsync(request.UserName, request.Password, ct);
            if (user is null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var response = _tokenService.GenerateToken(user);
            return Ok(response);
        }
    }
}
