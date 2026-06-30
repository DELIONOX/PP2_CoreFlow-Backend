using CoreFlow_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        // Método para mapear las claves explícitamente
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Clave primaria para cada una de las tablas
            modelBuilder.Entity<Cliente>().HasKey(c => c.IdCliente);
            modelBuilder.Entity<Proveedor>().HasKey(p => p.IdProveedor);
            modelBuilder.Entity<Producto>().HasKey(pr => pr.IdProducto);
            modelBuilder.Entity<Pedido>().HasKey(pe => pe.IdPedido);
        }
    }
}