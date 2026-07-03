using CoreFlow_Backend.Data;
using CoreFlow_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CoreFlow_Backend.Services
{
    public class CategoriaService
    {
        private readonly AppDbContext _context;

        public CategoriaService(AppDbContext context)
        {
            _context = context;
        }

        // =====================================
        // Métodos de Consulta (Queries)
        // =====================================
        public async Task<List<Categoria>> ObtenerTodos()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<Categoria?> ObtenerPorId(int id)
        {
            return await _context.Categorias.FindAsync(id);
        }

        // =====================================
        // Métodos de Persistencia 
        // =====================================
        public async Task<Categoria> Crear(Categoria categoria)
        {
            ValidarCategoria(categoria);
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<bool> Actualizar(int id, Categoria categoria)
        {
            var categoriaExistente = await _context.Categorias.FindAsync(id);
            if (categoriaExistente == null)
                return false;

            ValidarCategoria(categoria, id);

            categoriaExistente.NombreCategoria = categoria.NombreCategoria;
            categoriaExistente.Descripcion = categoria.Descripcion;
            categoriaExistente.Activa = categoria.Activa;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                return false;

            bool tieneProductos = await _context.Productos.AnyAsync(p => p.IdCategoria == id);
            if (tieneProductos)
            {
                throw new ValidationException("No se puede eliminar la categoría porque tiene productos asociados.");
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }

        // =====================================
        // Validaciones Internas
        // =====================================
        private void ValidarCategoria(Categoria categoria, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(categoria.NombreCategoria))
                throw new ValidationException("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(categoria.Descripcion))
                throw new ValidationException("La descripción es obligatoria.");

            bool existe = _context.Categorias.Any(c =>
                c.NombreCategoria.ToLower() == categoria.NombreCategoria.ToLower()
                && c.IdCategoria != id);

            if (existe)
                throw new ValidationException("Ya existe una categoría con ese nombre.");
        }
    }
}