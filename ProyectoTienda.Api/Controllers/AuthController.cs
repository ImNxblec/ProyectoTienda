using Microsoft.AspNetCore.Mvc;
using ProyectoTienda.Api.DTOs;
using ProyectoTienda.Api.Interfaces;

namespace ProyectoTienda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var result = await _authService.RegistrarAsync(dto);
            if (result.Contains("ya está registrado"))
                return BadRequest(new { mensaje = result });

            return Ok(new { token = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result.Contains("Credenciales incorrectas"))
                return Unauthorized(new { mensaje = result });

            return Ok(new { token = result });
        }
    }
}
