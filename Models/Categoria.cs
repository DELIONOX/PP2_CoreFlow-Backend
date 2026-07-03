using System.Text.Json.Serialization;

namespace CoreFlow_Backend.Models
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Activa { get; set; } = true;

        // =====================================
        // Propiedad de Navegación
        // =====================================
        [JsonIgnore]
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}