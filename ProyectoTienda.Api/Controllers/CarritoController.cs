using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTienda.Api.Data;
using ProyectoTienda.Api.Models;

namespace ProyectoTienda.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CarritoController : ControllerBase
	{
		private readonly AppDbContext _context;

		public CarritoController(AppDbContext context)
		{
			_context = context;
		}

		// GET: api/carrito/usuario/5
		[HttpGet("usuario/{usuarioId}")]
		public async Task<ActionResult<Carrito>> GetCarrito(int usuarioId)
		{
			var carrito = await _context.Carritos
				.Include(c => c.Items)
				.ThenInclude(i => i.Producto)
				.FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

			if (carrito == null)
			{
				carrito = new Carrito { UsuarioId = usuarioId };
				_context.Carritos.Add(carrito);
				await _context.SaveChangesAsync();
			}

			return carrito;
		}

		// POST: api/carrito/agregar
		[HttpPost("agregar")]
		public async Task<IActionResult> AgregarProducto([FromBody] CarritoItem item)
		{
			var carrito = await _context.Carritos
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.UsuarioId == item.UsuarioId);

			if (carrito == null)
			{
				carrito = new Carrito { UsuarioId = item.UsuarioId };
				_context.Carritos.Add(carrito);
				await _context.SaveChangesAsync();
			}

			var itemExistente = carrito.Items.FirstOrDefault(i => i.ProductoId == item.ProductoId);
			if (itemExistente != null)
			{
				itemExistente.Cantidad += item.Cantidad;
			}
			else
			{
				item.CarritoId = carrito.Id;
				_context.CarritoItems.Add(item);
			}

			await _context.SaveChangesAsync();
			return Ok();
		}

		// DELETE: api/carrito/item/5
		[HttpDelete("item/{itemId}")]
		public async Task<IActionResult> EliminarItem(int itemId)
		{
			var item = await _context.CarritoItems.FindAsync(itemId);
			if (item == null)
				return NotFound();

			_context.CarritoItems.Remove(item);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/carrito/vaciar/usuario/5
		[HttpDelete("vaciar/usuario/{usuarioId}")]
		public async Task<IActionResult> VaciarCarrito(int usuarioId)
		{
			var carrito = await _context.Carritos
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

			if (carrito == null)
				return NotFound();

			_context.CarritoItems.RemoveRange(carrito.Items);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
