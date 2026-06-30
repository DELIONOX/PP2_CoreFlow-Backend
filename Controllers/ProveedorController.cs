using CoreFlow_Backend.Models;
using CoreFlow_Backend.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult<List<Proveedor>>> Get()
        {
            return await _proveedorService.ObtenerTodos();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> Get(int id)
        {
            var proveedor =await _proveedorService.ObtenerPorId(id);

            if (proveedor == null)
            {
                return NotFound();
            }
            return proveedor;
        }

        [HttpPost]
        public async Task<ActionResult<Proveedor>>Post([FromBody] Proveedor proveedor)
        {
            var nuevo =await _proveedorService.Crear(proveedor);

            return Ok(nuevo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>Put(int id, [FromBody] Proveedor proveedor)
        {
            var actualizado =await _proveedorService.Actualizar(id, proveedor);
            if (!actualizado)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(int id)
        {
            var eliminado =await _proveedorService.Eliminar(id);
            if (!eliminado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}