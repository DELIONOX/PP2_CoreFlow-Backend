using CoreFlow_Backend.Models;
using CoreFlow_Backend.Services;
using Microsoft.AspNetCore.Mvc;

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

        // GET
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var productos = await _productoService.ObtenerTodos();
            
            // Proyectamos para forzar la salida de las propiedades calculadas en el JSON
            var respuesta = productos.Select(p => new
            {
                p.IdProducto,
                p.NombreProducto,
                p.Descripcion,
                p.Precio,
                p.Stock,
                p.IdProveedor,
                p.Estado,             
                p.NombreProveedor     
            });

            return Ok(respuesta);
        }

        // GET ID
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
                p.Estado,
                p.NombreProveedor
            };

            return Ok(respuesta);
        }

        // =====================================
        // Endpoints de Persistencia (POST, PUT, DELETE)
        // =====================================

        // POST
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Producto producto)
        {
            try
            {
                var nuevo = await _productoService.Crear(producto);
                return CreatedAtAction(
                    nameof(Get),
                    new { id = nuevo.IdProducto },
                    new { mensaje = "Producto registrado con éxito.", datos = nuevo }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Producto producto)
        {
            try
            {
                var actualizado = await _productoService.Actualizar(id, producto);
                if (!actualizado)
                {
                    return NotFound(new { mensaje = $"No se pudo actualizar. El producto con ID {id} no existe." });
                }

                return Ok(new { mensaje = "Producto actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _productoService.Eliminar(id);
            if (!eliminado)
            {
                return NotFound(new { mensaje = $"No se pudo eliminar. El producto con ID {id} no existe." });
            }

            return Ok(new { mensaje = "Producto eliminado correctamente." });
        }
    }
}