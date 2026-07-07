using CoreFlow_Backend.Models;
using CoreFlow_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CoreFlow_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorController : ControllerBase
    {
        private readonly ProveedorService _proveedorService;

        public ProveedorController(ProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        // =====================================
        // Métodos de Consulta (Queries)
        // =====================================

        // GET: api/Proveedor
        [HttpGet]
        public async Task<ActionResult<List<Proveedor>>> Get()
        {
            var proveedores = await _proveedorService.ObtenerTodos();
            return Ok(proveedores);
        }

        // GET: api/Proveedor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> Get(int id)
        {
            var proveedor = await _proveedorService.ObtenerPorId(id);

            if (proveedor == null)
            {
                return NotFound(new { mensaje = "Proveedor no encontrado." });
            }

            return Ok(proveedor);
        }

        // =====================================
        // Métodos de Persistencia (Mutaciones)
        // =====================================

        // POST: api/Proveedor
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Proveedor proveedor)
        {
            try
            {
                var nuevo = await _proveedorService.Crear(proveedor);
                return Ok(new { mensaje = "Proveedor registrado correctamente.", proveedor = nuevo });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // PUT: api/Proveedor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Proveedor proveedor)
        {
            try
            {
                var actualizado = await _proveedorService.Actualizar(id, proveedor);

                if (!actualizado)
                {
                    return NotFound(new { mensaje = "Proveedor no encontrado." });
                }

                return Ok(new { mensaje = "Proveedor actualizado correctamente." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // DELETE: api/Proveedor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _proveedorService.Eliminar(id);

                if (!eliminado)
                {
                    return NotFound(new { mensaje = "Proveedor no encontrado." });
                }

                return Ok(new { mensaje = "Proveedor eliminado correctamente." });
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { mensaje = "No se puede eliminar el proveedor porque tiene productos asociados." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}