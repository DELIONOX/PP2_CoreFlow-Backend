using CoreFlow_Backend.Models;
using CoreFlow_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CoreFlow_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _pedidoService;

        public PedidoController(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        // GET: api/Pedido
        [HttpGet]
        public async Task<ActionResult<List<Pedido>>> Get()
        {
            return await _pedidoService.ObtenerTodos();
        }

        // GET: api/Pedido/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> Get(int id)
        {
            var pedido = await _pedidoService.ObtenerPorId(id);

            if (pedido == null)
            {
                return NotFound(new { mensaje = "Pedido no encontrado." });
            }

            return pedido;
        }

        // POST: api/Pedido
        [HttpPost]
        public async Task<ActionResult<Pedido>> Post([FromBody] Pedido pedido)
        {
            try
            {
                var nuevo = await _pedidoService.Crear(pedido);
                return Ok(new { mensaje = "Pedido registrado correctamente.", pedido = nuevo });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // PUT: api/Pedido/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Pedido pedido)
        {
            try
            {
                var actualizado = await _pedidoService.Actualizar(id, pedido);

                if (!actualizado)
                {
                    return NotFound(new { mensaje = "Pedido no encontrado." });
                }

                return Ok(new { mensaje = "Pedido actualizado correctamente." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // DELETE: api/Pedido/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _pedidoService.Eliminar(id);

                if (!eliminado)
                {
                    return NotFound(new { mensaje = "Pedido no encontrado." });
                }

                return Ok(new { mensaje = "Pedido eliminado correctamente." });
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { mensaje = "No se puede eliminar el pedido porque tiene registros dependientes activos." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}