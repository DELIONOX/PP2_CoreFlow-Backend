using CoreFlow_Backend.Models;
using CoreFlow_Backend.Services;
using Microsoft.AspNetCore.Mvc;
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

        // GET
        [HttpGet]
        public async Task<ActionResult<List<Proveedor>>> Get()
        {
            var proveedores = await _proveedorService.ObtenerTodos();
            return Ok(proveedores);
        }

        // GET ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> Get(int id)
        {
            var proveedor = await _proveedorService.ObtenerPorId(id);

            if (proveedor == null)
            {
                return NotFound(new
                {
                    mensaje = $"No existe el proveedor con ID {id}."
                });
            }

            return Ok(proveedor);
        }

        // =====================================
        // Métodos de Persistencia (Mutaciones)
        // =====================================

        // POST
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Proveedor proveedor)
        {
            try
            {
                var nuevo = await _proveedorService.Crear(proveedor);

                return CreatedAtAction(
                    nameof(Get),
                    new { id = nuevo.IdProveedor },
                    new
                    {
                        mensaje = "Proveedor registrado correctamente.",
                        datos = nuevo
                    });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Proveedor proveedor)
        {
            try
            {
                var actualizado = await _proveedorService.Actualizar(id, proveedor);

                if (!actualizado)
                {
                    return NotFound(new
                    {
                        mensaje = $"No existe el proveedor con ID {id}."
                    });
                }

                return Ok(new
                {
                    mensaje = "Proveedor actualizado correctamente."
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _proveedorService.Eliminar(id);

                if (!eliminado)
                {
                    return NotFound(new
                    {
                        mensaje = $"No existe el proveedor con ID {id}."
                    });
                }

                return Ok(new
                {
                    mensaje = "Proveedor eliminado correctamente."
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }
    }
}