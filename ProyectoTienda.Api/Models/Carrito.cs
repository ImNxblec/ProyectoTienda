namespace ProyectoTienda.Api.Models
{
    public class Carrito
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public List<CarritoItem> Items { get; set; } = new();

        public ICollection<CarritoItem> CarritoItems { get; set; } = new List<CarritoItem>();

    }
}
