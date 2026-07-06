using CoreFlow_Backend.Data;
using CoreFlow_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CoreFlow_Backend.Services
{
    public class ProveedorService
    {
        private readonly AppDbContext _context;

        public ProveedorService(AppDbContext context)
        {
            _context = context;
        }

        // =====================================
        // Métodos de Consulta (Queries)
        // =====================================

        // GET
        public async Task<List<Proveedor>> ObtenerTodos()
        {
            return await _context.Proveedores.ToListAsync();
        }

        // GET ID
        public async Task<Proveedor?> ObtenerPorId(int id)
        {
            return await _context.Proveedores.FindAsync(id);
        }

        // =====================================
        // Métodos de Persistencia (Mutaciones)
        // =====================================

        // POST
        public async Task<Proveedor> Crear(Proveedor proveedor)
        {
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            return proveedor;
        }

        // PUT
        public async Task<bool> Actualizar(int id, Proveedor proveedor)
        {
            var proveedorExistente = await _context.Proveedores.FindAsync(id);

            if (proveedorExistente == null)
            {
                return false;
            }

            proveedorExistente.NombreEmpresa = proveedor.NombreEmpresa;
            proveedorExistente.Contacto = proveedor.Contacto;
            proveedorExistente.Correo = proveedor.Correo;
            proveedorExistente.Telefono = proveedor.Telefono;

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE
        public async Task<bool> Eliminar(int id)
        {
            // Verificar si el proveedor está siendo utilizado por algún producto
            bool estaEnUso = await _context.Productos.AnyAsync(p => p.IdProveedor == id);

            if (estaEnUso)
            {
                throw new ValidationException(
                    "No se puede eliminar el proveedor porque está siendo utilizado por uno o más productos."
                );
            }

            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
            {
                return false;
            }

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}