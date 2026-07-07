using CoreFlow_Backend.Models;
using CoreFlow_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CoreFlow_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // =====================================
        // Métodos de Consulta (Queries)
        // =====================================

        // GET: api/Cliente
        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> Get()
        {
            return await _clienteService.ObtenerTodos();
        }

        // GET: api/Cliente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> Get(int id)
        {
            var cliente = await _clienteService.ObtenerPorId(id);

            if (cliente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado." });
            }

            return cliente;
        }

        // =====================================
        // Métodos de Persistencia (Mutaciones)
        // =====================================

        // POST: api/Cliente
        [HttpPost]
        public async Task<ActionResult<Cliente>> Post([FromBody] Cliente cliente)
        {
            try
            {
                var nuevo = await _clienteService.Crear(cliente);
                return Ok(new { mensaje = "Cliente registrado correctamente.", cliente = nuevo });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // PUT: api/Cliente/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Cliente cliente)
        {
            try
            {
                var actualizado = await _clienteService.Actualizar(id, cliente);

                if (!actualizado)
                {
                    return NotFound(new { mensaje = "Cliente no encontrado." });
                }

                return Ok(new { mensaje = "Cliente actualizado correctamente." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _clienteService.Eliminar(id);

                if (!eliminado)
                {
                    return NotFound(new { mensaje = "Cliente no encontrado." });
                }

                return Ok(new { mensaje = "Cliente eliminado correctamente." });
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { mensaje = "No se puede eliminar el cliente porque tiene pedidos registrados asociados." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}