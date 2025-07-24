namespace ProyectoTienda.Api.Models
{
    public class CarritoItem
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        public int Cantidad { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; } = null!;

    }
}
