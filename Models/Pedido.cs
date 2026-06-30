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
     }
}