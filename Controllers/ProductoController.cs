using CoreFlow_Backend.Models;
using CoreFlow_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CoreFlow_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        // =====================================
        // Endpoints de Consulta (GET)
        // =====================================

        // GET: api/Producto
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var productos = await _productoService.ObtenerTodos();
            var respuesta = productos.Select(p => new
            {
                p.IdProducto,
                p.NombreProducto,
                p.Descripcion,
                p.Precio,
                p.Stock,
                p.IdProveedor,
                p.NombreProveedor,
                p.IdCategoria,
                p.NombreCategoria,
                p.Estado
            });

            return Ok(respuesta);
        }

        // GET: api/Producto/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var p = await _productoService.ObtenerPorId(id);

            if (p == null)
            {
                return NotFound(new { mensaje = $"El producto con ID {id} no fue encontrado." });
            }

            var respuesta = new
            {
                p.IdProducto,
                p.NombreProducto,
                p.Descripcion,
                p.Precio,
                p.Stock,
                p.IdProveedor,
                p.NombreProveedor,
                p.IdCategoria,
                p.NombreCategoria,
                p.Estado
            };

            return Ok(respuesta);
        }

        // =====================================
        // Endpoints de Persistencia
        // =====================================

        // POST: api/Producto
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Producto producto)
        {
            try
            {
                var nuevo = await _productoService.Crear(producto);
                return Ok(new { mensaje = "Producto registrado correctamente.", producto = nuevo });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // PUT: api/Producto/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Producto producto)
        {
            try
            {
                var actualizado = await _productoService.Actualizar(id, producto);

                if (!actualizado)
                {
                    return NotFound(new { mensaje = $"El producto con ID {id} no existe." });
                }

                return Ok(new { mensaje = "Producto actualizado correctamente." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // DELETE: api/Producto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _productoService.Eliminar(id);

                if (!eliminado)
                {
                    return NotFound(new { mensaje = "Producto no encontrado." });
                }

                return Ok(new { mensaje = "Producto eliminado correctamente." });
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { mensaje = "No se puede eliminar el producto porque está incluido en detalles de pedidos activos." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}