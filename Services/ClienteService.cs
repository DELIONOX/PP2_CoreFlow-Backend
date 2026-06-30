using CoreFlow_Backend.Data;
using CoreFlow_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow_Backend.Services
{
    public class ClienteService
    {
        private readonly AppDbContext _context;

        public ClienteService(AppDbContext context)
        {
            _context = context;
        }

        //GET
        public async Task<List<Cliente>> ObtenerTodos()
        {
            return await _context.Clientes.ToListAsync();
        }

        //GET ID
        public async Task<Cliente?> ObtenerPorId(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        //POST
        public async Task<Cliente> Crear(Cliente cliente)
        {
            var existeCorreo = await _context.Clientes
                .AnyAsync(c => c.Correo == cliente.Correo);
            if (existeCorreo)
            {
                throw new Exception("Ya existe un cliente con ese correo.");
            }

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        //PUT
        public async Task<bool> Actualizar(int id, Cliente cliente)
        {
            var clienteExistente = await _context.Clientes.FindAsync(id);
            if (clienteExistente == null)
            {
                return false;
            }
            var existeCorreo = await _context.Clientes.AnyAsync(c =>
                c.Correo == cliente.Correo &&
                c.IdCliente != id);
            if (existeCorreo)
            {
                throw new Exception("Ya existe un cliente con ese correo.");
            }

            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.Apellido = cliente.Apellido;
            clienteExistente.Correo = cliente.Correo;
            clienteExistente.Telefono = cliente.Telefono;

            await _context.SaveChangesAsync();
            return true;
        }

        //DELETE
        public async Task<bool> Eliminar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return false;
            }
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}