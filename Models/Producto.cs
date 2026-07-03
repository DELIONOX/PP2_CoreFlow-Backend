using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        public int IdCategoria { get; set; }

        // =====================================
        // Propiedades de Navegación
        // =====================================
        [ForeignKey(nameof(IdProveedor))]
        [JsonIgnore]
        public Proveedor? Proveedor { get; set; }

        [ForeignKey(nameof(IdCategoria))]
        [JsonIgnore]
        public Categoria? Categoria { get; set; }

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

        [NotMapped]
        public string NombreCategoria
        {
            get { return Categoria?.NombreCategoria ?? ""; }
        }
    }
}