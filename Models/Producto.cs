using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // <-- Asegúrate de agregar este using

namespace CoreFlow_Backend.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int IdProveedor { get; set; }

        // =====================================
        // Propiedad de Navegación
        // =====================================
        [ForeignKey(nameof(IdProveedor))]
        [JsonIgnore] // <-- AGREGA ESTO AQUÍ
        public Proveedor? Proveedor { get; set; }

        // =====================================
        // Propiedades Calculadas (No Mapeadas)
        // =====================================
        [NotMapped]
        public string Estado
        {
            get { return Stock > 0 ? "Disponible" : "Sin stock"; }
        }

        [NotMapped]
        public string NombreProveedor
        {
            get { return Proveedor?.NombreEmpresa ?? ""; }
        }
    }
}