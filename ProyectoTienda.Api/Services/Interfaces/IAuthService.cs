using ProyectoTienda.Api.DTOs;
using ProyectoTienda.Api.Models;

namespace ProyectoTienda.Api.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegistrarAsync(RegisterDTO dto);
        Task<string> LoginAsync(LoginDTO dto);
    }
}
