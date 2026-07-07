using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CoreFlow_Backend.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public DateTime FechaPedido { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public int IdCliente { get; set; }
        public int IdProducto { get; set; }

        // =====================================
        // Propiedades de Navegación
        // =====================================
        [ForeignKey(nameof(IdCliente))]
        [JsonIgnore]
        public Cliente? Cliente { get; set; }

        [ForeignKey(nameof(IdProducto))]
        [JsonIgnore]
        public Producto? Producto { get; set; }
    }
}