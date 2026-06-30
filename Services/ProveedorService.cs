using CoreFlow_Backend.Data;
using CoreFlow_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow_Backend.Services
{
    public class ProveedorService
    {
        private readonly AppDbContext _context;
        public ProveedorService(AppDbContext context)
        {
            _context=context;
        }
        //GET
        public async Task<List<Proveedor>> ObtenerTodos()
        {
            return await _context.Proveedores.ToListAsync();
        }

        //GET ID
        public async Task<Proveedor?> ObtenerPorId(int id)
        {
            return await _context.Proveedores.FindAsync(id);
        }

        //POST
        public async Task<Proveedor> Crear(Proveedor proveedor)
        {
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            return proveedor;
        }

        //PUT
        public async Task<bool> Actualizar(int id, Proveedor proveedor)
        {
            var proveedorExistente =
                await _context.Proveedores.FindAsync(id);

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

        //DELETE
        public async Task<bool> Eliminar(int id)
        {
            var proveedor =await _context.Proveedores.FindAsync(id);

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