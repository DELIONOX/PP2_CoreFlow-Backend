using CoreFlow_Backend.Data;
using CoreFlow_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow_Backend.Services
{
    public class PedidoService
    {
        private readonly AppDbContext _context;

        public PedidoService(AppDbContext context)
        {
            _context = context;
        }

        // GET: Obtener todos los pedidos
        public async Task<List<Pedido>> ObtenerTodos()
        {
            return await _context.Pedidos.ToListAsync();
        }

        // GET: Obtener un pedido por ID
        public async Task<Pedido?> ObtenerPorId(int id)
        {
            return await _context.Pedidos.FindAsync(id);
        }

        // POST: Crear un nuevo pedido
        public async Task<Pedido> Crear(Pedido pedido)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.IdProducto == pedido.IdProducto);

            if (producto == null)
                throw new Exception("El producto no existe.");

            if (producto.Stock < pedido.Cantidad)
                throw new Exception("Stock insuficiente.");

            // Cálculos y actualización de stock
            pedido.Total = producto.Precio * pedido.Cantidad;
            producto.Stock -= pedido.Cantidad;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }

        // PUT: Actualizar un pedido existente
        public async Task<bool> Actualizar(int id, Pedido pedido)
        {
            var pedidoExistente = await _context.Pedidos.FindAsync(id);
            if (pedidoExistente == null) return false;

            var productoNuevo = await _context.Productos
                .FirstOrDefaultAsync(p => p.IdProducto == pedido.IdProducto);

            if (productoNuevo == null)
                throw new Exception("El producto no existe.");

            // Devolver stock al producto anterior si aplica
            var productoAnterior = await _context.Productos
                .FirstOrDefaultAsync(p => p.IdProducto == pedidoExistente.IdProducto);

            if (productoAnterior != null)
                productoAnterior.Stock += pedidoExistente.Cantidad;

            // Validar y descontar stock del nuevo producto
            if (productoNuevo.Stock < pedido.Cantidad)
                throw new Exception("Stock insuficiente.");

            productoNuevo.Stock -= pedido.Cantidad;

            // Mapeo de datos actualizados
            pedidoExistente.IdCliente = pedido.IdCliente;
            pedidoExistente.IdProducto = pedido.IdProducto;
            pedidoExistente.FechaPedido = pedido.FechaPedido;
            pedidoExistente.Cantidad = pedido.Cantidad;
            pedidoExistente.Total = productoNuevo.Precio * pedido.Cantidad;

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE: Eliminar un pedido
        public async Task<bool> Eliminar(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return false;

            // Devolver el stock retenido por el pedido
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.IdProducto == pedido.IdProducto);

            if (producto != null)
                producto.Stock += pedido.Cantidad;

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}