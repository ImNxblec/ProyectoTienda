using Microsoft.EntityFrameworkCore;
using ProyectoTienda.Api.Models;

namespace ProyectoTienda.Api.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Usuario> Usuarios => Set<Usuario>();
		public DbSet<Categoria> Categorias { get; set; }
		public DbSet<Producto> Productos { get; set; }
		public DbSet<CarritoItem> CarritoItems { get; set; }
		public DbSet<Carrito> Carritos { get; set; }
		public DbSet<Factura> Facturas { get; set; }
		public DbSet<DetalleFactura> DetallesFactura { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Relación Carrito -> Usuario (sin cascada para evitar rutas múltiples)
			modelBuilder.Entity<Carrito>()
				.HasOne(c => c.Usuario)
				.WithMany()
				.HasForeignKey(c => c.UsuarioId)
				.OnDelete(DeleteBehavior.Restrict); // CAMBIO CLAVE

			// Relación CarritoItems -> Carrito
			modelBuilder.Entity<CarritoItem>()
				.HasOne(ci => ci.Carrito)
				.WithMany(c => c.Items)
				.HasForeignKey(ci => ci.CarritoId)
				.OnDelete(DeleteBehavior.Cascade); // ok

			// Relación CarritoItems -> Producto
			modelBuilder.Entity<CarritoItem>()
				.HasOne(ci => ci.Producto)
				.WithMany()
				.HasForeignKey(ci => ci.ProductoId)
				.OnDelete(DeleteBehavior.Restrict); // evitar cascada

			// Relación CarritoItems -> Usuario (sin cascada)
			modelBuilder.Entity<CarritoItem>()
				.HasOne(ci => ci.Usuario)
				.WithMany()
				.HasForeignKey(ci => ci.UsuarioId)
				.OnDelete(DeleteBehavior.Restrict); // CAMBIO CLAVE
		}

	}
}
