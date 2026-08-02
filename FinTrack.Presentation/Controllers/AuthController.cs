using FinTrack.Application.DTOs.User;
using FinTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            if (!result)
            {
                return BadRequest(new { message = "Bu e-posta adresi ile zaten bir kayıt bulunuyor." });
            }

            return Ok(new { message = "Kayıt işlemi başarıyla gerçekleştirildi." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await _authService.LoginAsync(request);

            if (response == null)
            {
                return Unauthorized(new { message = "E-posta veya şifre hatalı." });
            }

            return Ok(response);
        }
    }
}