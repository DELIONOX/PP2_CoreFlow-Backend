using CoreFlow_Backend.Models;
using CoreFlow_Backend.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult<List<Pedido>>> Get()
        {
            return await _pedidoService.ObtenerTodos();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> Get(int id)
        {
            var pedido = await _pedidoService.ObtenerPorId(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return pedido;
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>>Post([FromBody] Pedido pedido)
        {
            var nuevo =await _pedidoService.Crear(pedido);

            return Ok(nuevo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>Put(int id, [FromBody] Pedido pedido)
        {
            var actualizado =await _pedidoService.Actualizar(id, pedido);
            if (!actualizado)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult>Delete(int id)
        {
            var eliminado = await _pedidoService.Eliminar(id);
            if (!eliminado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}