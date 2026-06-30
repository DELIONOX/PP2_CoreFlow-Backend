using System.ComponentModel.DataAnnotations;

namespace CoreFlow_Backend.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } =string.Empty;
        public string Apellido { get; set; }=string.Empty;
        public string Correo { get; set; }=string.Empty;
        public string Telefono { get; set; }=string.Empty;
    }
}

