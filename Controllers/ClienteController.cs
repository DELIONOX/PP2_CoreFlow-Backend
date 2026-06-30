using CoreFlow_Backend.Models;
using CoreFlow_Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreFlow_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ClienteController : ControllerBase
{
    private readonly ClienteService _clienteService;
    public ClienteController(ClienteService clienteService)
    {
        _clienteService =clienteService;
    }

    //GET
    [HttpGet]
    public async Task<ActionResult<List<Cliente>>> GetTodos()
    {
        return await _clienteService.ObtenerTodos();
    }

    //GET por ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> GetId(int id)
    {
        var cliente= await _clienteService.ObtenerPorId(id);
        if(cliente == null)
        {
            return NotFound();
        }
        return cliente;
    }

    //POST
    [HttpPost]
    public async Task<ActionResult<Cliente>>Post([FromBody]Cliente cliente)
    {
        var nuevoCliente=await _clienteService.Crear(cliente);
        return Ok(nuevoCliente);
    }

    //PUT
    [HttpPut("{id}")]
    public async Task<IActionResult>Put(int id, [FromBody] Cliente cliente)
    {
        var actualizarCliente=await _clienteService.Actualizar(id, cliente);
        if (!actualizarCliente)
        {
            return NotFound();
        }
        return NoContent();
    }

    //DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult>Delete(int id)
    {
        var eliminarPorID= await _clienteService.Eliminar(id);
        if (!eliminarPorID)
        {
            return NotFound();
        }
        return NoContent();
    }
    
}

