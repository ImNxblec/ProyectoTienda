using Microsoft.EntityFrameworkCore;
using ProyectoTienda.Api.Data;
using ProyectoTienda.Api.Models;
using ProyectoTienda.Api.Repositories.Interfaces;
using ProyectoTienda.Api.Repositories.Implementaciones; // <-- Asegúrate de tener esto

namespace ProyectoTienda.Api.Repositories.Implementaciones
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
