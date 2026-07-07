using CoreFlow_Backend.Models;
using CoreFlow_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: api/Categoria
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            return await _categoriaService.ObtenerTodos();
        }

        // GET: api/Categoria/5
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

        // POST: api/Categoria
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

        // PUT: api/Categoria/5
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

        // DELETE: api/Categoria/5
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
            catch (DbUpdateException)
            {
                return BadRequest(new { mensaje = "No se puede eliminar la categoría porque tiene productos asociados." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}