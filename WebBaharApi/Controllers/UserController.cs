using Bahar.Application.Dto.SigninVLogin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace WebBaharApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AuthenticationService _authService;

        public UserController(AuthenticationService authService)
        {
            _authService = authService;
        }

        
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RegisterUserRequestDto request)
        {
            if (request == null)
                return BadRequest("Request body is null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _authService.Register(request);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { userId = result.Data.UserId, message = "ثبت‌نام با موفقیت انجام شد." });
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            if (request == null)
                return BadRequest("Request body is null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _authService.Login(request);
            if (!result.IsSuccess)
                return Unauthorized(new { message = result.Message });

            return Ok(result.Data);
        }
    }
}
