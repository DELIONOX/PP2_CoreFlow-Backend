using CoreFlow_Backend.Data;
using CoreFlow_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CoreFlow_Backend.Services
{
    public class PedidoService
    {
        private readonly AppDbContext _context;

        public PedidoService(AppDbContext context)
        {
            _context = context;
        }

        // =====================================
        // Métodos de Consulta (Queries)
        // =====================================

        public async Task<List<Pedido>> ObtenerTodos()
        {
            return await _context.Pedidos.ToListAsync();
        }

        public async Task<Pedido?> ObtenerPorId(int id)
        {
            return await _context.Pedidos.FindAsync(id);
        }

        // =====================================
        // Métodos de Persistencia (Mutaciones)
        // =====================================

        public async Task<Pedido> Crear(Pedido pedido)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.IdProducto == pedido.IdProducto);

            if (producto == null)
            {
                throw new ValidationException("El producto seleccionado no existe.");
            }

            if (producto.Stock < pedido.Cantidad)
            {
                throw new ValidationException($"Stock insuficiente para el producto: {producto.NombreProducto}.");
            }

            pedido.Total = producto.Precio * pedido.Cantidad;
            producto.Stock -= pedido.Cantidad;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<bool> Actualizar(int id, Pedido pedido)
        {
            var pedidoExistente = await _context.Pedidos.FindAsync(id);

            if (pedidoExistente == null)
            {
                return false;
            }

            var productoNuevo = await _context.Productos.FirstOrDefaultAsync(p => p.IdProducto == pedido.IdProducto);

            if (productoNuevo == null)
            {
                throw new ValidationException("El producto seleccionado no existe.");
            }

            var productoAnterior = await _context.Productos.FirstOrDefaultAsync(p => p.IdProducto == pedidoExistente.IdProducto);

            if (productoAnterior != null)
            {
                productoAnterior.Stock += pedidoExistente.Cantidad;
            }

            if (productoNuevo.Stock < pedido.Cantidad)
            {
                if (productoAnterior != null)
                {
                    productoAnterior.Stock -= pedidoExistente.Cantidad;
                }

                throw new ValidationException($"Stock insuficiente para el producto: {productoNuevo.NombreProducto}.");
            }

            productoNuevo.Stock -= pedido.Cantidad;

            pedidoExistente.IdCliente = pedido.IdCliente;
            pedidoExistente.IdProducto = pedido.IdProducto;
            pedidoExistente.FechaPedido = pedido.FechaPedido;
            pedidoExistente.Cantidad = pedido.Cantidad;
            pedidoExistente.Total = productoNuevo.Precio * pedido.Cantidad;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return false;
            }

            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.IdProducto == pedido.IdProducto);

            if (producto != null)
            {
                producto.Stock += pedido.Cantidad;
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}