using CoreFlow_Backend.Data;
using CoreFlow_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CoreFlow_Backend.Services
{
    public class ProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        // =====================================
        // Métodos de Consulta (Queries)
        // =====================================

        // GET
        public async Task<List<Producto>> ObtenerTodos()
        {
            return await _context.Productos
                .Include(p => p.Proveedor)
                .ToListAsync();
        }

        // GET ID
        public async Task<Producto?> ObtenerPorId(int id)
        {
            return await _context.Productos
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(p => p.IdProducto == id);
        }

        // =====================================
        // Métodos de Persistencia (Mutaciones)
        // =====================================

        // POST
        public async Task<Producto> Crear(Producto producto)
        {
            ValidarProducto(producto);

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return producto;
        }

        // PUT
        public async Task<bool> Actualizar(int id, Producto producto)
        {
            var productoExistente = await _context.Productos.FindAsync(id);

            if (productoExistente == null)
            {
                return false;
            }

            ValidarProducto(producto, id);

            productoExistente.NombreProducto = producto.NombreProducto;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio = producto.Precio;
            productoExistente.Stock = producto.Stock;
            productoExistente.IdProveedor = producto.IdProveedor;

            await _context.SaveChangesAsync();

            return true;
        }

        // DELETE
        public async Task<bool> Eliminar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return false;
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return true;
        }

        // =====================================
        // VALIDACIONES
        // =====================================

        private void ValidarProducto(Producto producto, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(producto.NombreProducto))
                throw new ValidationException("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(producto.Descripcion))
                throw new ValidationException("La descripción es obligatoria.");

            if (producto.Precio <= 0)
                throw new ValidationException("El precio debe ser mayor que cero.");

            if (producto.Stock < 0)
                throw new ValidationException("El stock no puede ser negativo.");

            if (producto.IdProveedor <= 0)
                throw new ValidationException("Seleccione un proveedor válido.");

            bool proveedorExiste = _context.Proveedores
                .Any(p => p.IdProveedor == producto.IdProveedor);

            if (!proveedorExiste)
                throw new ValidationException("El proveedor seleccionado no existe.");

            bool productoDuplicado = _context.Productos.Any(p =>
                p.NombreProducto.ToLower() == producto.NombreProducto.ToLower()
                && p.IdProducto != id);

            if (productoDuplicado)
                throw new ValidationException("Ya existe un producto con ese nombre.");
        }
    }
}