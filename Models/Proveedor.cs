namespace CoreFlow_Backend.Models
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string NombreEmpresa { get; set; }=string.Empty;
        public string Contacto { get; set; }=string.Empty;
        public string Correo { get; set; } =string.Empty;
        public string Telefono { get; set; }=string.Empty;
    }
}