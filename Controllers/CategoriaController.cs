using CoreFlow_Backend.Models;
using CoreFlow_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CoreFlow_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriaController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            return await _categoriaService.ObtenerTodos();
        }

        // GET ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            var categoria = await _categoriaService.ObtenerPorId(id);

            if (categoria == null)
            {
                return NotFound(new { mensaje = "Categoría no encontrada." });
            }

            return categoria;
        }

        // POST
        [HttpPost]
        public async Task<ActionResult<Categoria>> Post([FromBody] Categoria categoria)
        {
            try
            {
                var nueva = await _categoriaService.Crear(categoria);
                return Ok(new
                {
                    mensaje = "Categoría registrada correctamente.",
                    categoria = nueva
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Categoria categoria)
        {
            try
            {
                var actualizado = await _categoriaService.Actualizar(id, categoria);

                if (!actualizado)
                {
                    return NotFound(new { mensaje = "Categoría no encontrada." });
                }

                return Ok(new { mensaje = "Categoría actualizada correctamente." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _categoriaService.Eliminar(id);

                if (!eliminado)
                {
                    return NotFound(new { mensaje = "Categoría no encontrada." });
                }

                return Ok(new { mensaje = "Categoría eliminada correctamente." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}