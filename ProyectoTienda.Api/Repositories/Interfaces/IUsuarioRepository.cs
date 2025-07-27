using ProyectoTienda.Api.Models;

namespace ProyectoTienda.Api.Repositories.Interfaces
{
	public interface IUsuarioRepository : IGenericRepository<Usuario>
	{
		Task<Usuario?> GetByEmailAsync(string email);
	}
}
