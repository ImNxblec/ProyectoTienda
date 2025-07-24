using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoTienda.Api.Data;
using ProyectoTienda.Api.DTOs;
using ProyectoTienda.Api.Interfaces;
using ProyectoTienda.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProyectoTienda.Api.Services
{
	public class AuthService : IAuthService
	{
		private readonly AppDbContext _context;
		private readonly IConfiguration _config;

		public AuthService(AppDbContext context, IConfiguration config)
		{
			_context = context;
			_config = config;
		}

		public async Task<string> RegistrarAsync(RegisterDTO dto)
		{
			if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
				return "El correo ya está registrado.";

			var user = new Usuario
			{
				Nombre = dto.Nombre,
				Email = dto.Email,
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
			};

			_context.Usuarios.Add(user);
			await _context.SaveChangesAsync();

			return GenerarToken(user);
		}

		public async Task<string> LoginAsync(LoginDTO dto)
		{
			var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
			if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
				return "Credenciales incorrectas.";

			return GenerarToken(user);
		}

		private string GenerarToken(Usuario user)
		{
			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.Nombre),
				new Claim(ClaimTypes.Email, user.Email)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _config["Jwt:Issuer"],
				claims: claims,
				expires: DateTime.UtcNow.AddHours(2),
				signingCredentials: creds
			);



			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
