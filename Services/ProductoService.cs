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

        public async Task<List<Producto>> ObtenerTodos()
        {
            return await _context.Productos
                .Include(p => p.Proveedor)
                .Include(p => p.Categoria)
                .ToListAsync();
        }

        public async Task<Producto?> ObtenerPorId(int id)
        {
            return await _context.Productos
                .Include(p => p.Proveedor)
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.IdProducto == id);
        }

        // =====================================
        // Métodos de Persistencia (Mutaciones)
        // =====================================

        public async Task<Producto> Crear(Producto producto)
        {
            ValidarProducto(producto);

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return await _context.Productos
                .Include(p => p.Proveedor)
                .Include(p => p.Categoria)
                .FirstAsync(p => p.IdProducto == producto.IdProducto);
        }

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
            productoExistente.IdCategoria = producto.IdCategoria;

            await _context.SaveChangesAsync();
            return true;
        }

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
        // Validaciones Internas
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

            if (producto.IdCategoria <= 0)
                throw new ValidationException("Seleccione una categoría válida.");

            bool categoriaExiste = _context.Categorias
                .Any(c => c.IdCategoria == producto.IdCategoria);

            if (!categoriaExiste)
                throw new ValidationException("La categoría seleccionada no existe.");

            bool productoDuplicado = _context.Productos.Any(p =>
                p.NombreProducto.ToLower() == producto.NombreProducto.ToLower()
                && p.IdProducto != id);

            if (productoDuplicado)
                throw new ValidationException("Ya existe un producto con ese nombre.");
        }
    }
}