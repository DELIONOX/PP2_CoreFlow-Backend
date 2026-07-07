using CoreFlow_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // =====================================
        // DbSets (Tablas)
        // =====================================
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        // =====================================
        // Configuración del Modelo (Fluent API)
        // =====================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Claves Primarias
            modelBuilder.Entity<Cliente>().HasKey(c => c.IdCliente);
            modelBuilder.Entity<Proveedor>().HasKey(p => p.IdProveedor);
            modelBuilder.Entity<Categoria>().HasKey(c => c.IdCategoria);
            modelBuilder.Entity<Producto>().HasKey(p => p.IdProducto);
            modelBuilder.Entity<Pedido>().HasKey(p => p.IdPedido);

            // Relación: Producto -> Proveedor
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Proveedor)
                .WithMany(pr => pr.Productos)
                .HasForeignKey(p => p.IdProveedor)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación: Producto -> Categoría
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación: Pedido -> Cliente
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación: Pedido -> Producto
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Producto)
                .WithMany(pr => pr.Pedidos)
                .HasForeignKey(p => p.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}