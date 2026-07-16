using Microsoft.AspNetCore.Mvc;
using OrderService.Interfaces;
using Shared.Contracts.DTOs.Projec;
namespace OrderService.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProyectosController : ControllerBase
    {
        private readonly IProyectoService _proyectoService;

        public ProyectosController(IProyectoService proyectoService)
        {
            _proyectoService = proyectoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var proyectos = await _proyectoService.GetAllProyectosAsync();
            return Ok(proyectos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var proyecto = await _proyectoService.GetProyectoByIdAsync(id);
            if (proyecto == null)
            {
                return NotFound(new { message = "Projecto no encontrado" });
            }
            return Ok(proyecto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProyectoDto dto)
        {
            try
            {
                var proyecto = await _proyectoService.CreateAsync(dto);
                return StatusCode(201, dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProyectoDto dto)
        {
            var actualizado = await _proyectoService.UpdateAsync(id, dto);
            if (actualizado == null)
            {
                return NotFound(new { message = "proyecto no encontrado" });
            }
            return Ok(actualizado);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var exito = await _proyectoService.DeleteAsync(id);
            if (exito == null)
            {
                return NotFound(new { message = "Proyecto No encontrado" });
            }
            return Ok(new { message = "Proyecto eliminado con exito" });
        }
    }
}
