using Microsoft.AspNetCore.Mvc;
using OrderService.Controllers;
using OrderService.Interfaces;
using Shared.Contracts.DTOs.Propuestas;


namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropuestasController : ControllerBase
    {
        private readonly IPropuestaService _propuestaService;

        public PropuestasController(IPropuestaService propuestaService)
        {
            _propuestaService = propuestaService;
        }

        [HttpGet("proyecto/{proyectoId}")]
        public async Task<IActionResult> GetByProyecto(int proyectoId)
        {
            var p = await _propuestaService.GetByProyectoIdAsync(proyectoId);
            return Ok(p);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var p = await _propuestaService.GetByIdAsync(id);
            if (p == null)
            {
                return NotFound(new { message = "Propuesta no encontra" });
            }
            return Ok(p);
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CreatePropuestaDto dto)
        {
            try
            {
                var np = await _propuestaService.CreateAsync(dto);
                return StatusCode(201, np);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/aceptar")]
        public async Task<IActionResult> AceptarPropuesta(int id)
        {
            var actualizada = await _propuestaService.ResponderPropuestaAsync(id, aceptada: true);
            if (actualizada == null) return NotFound(new { message = "Propuesta no encontrada." });

            return Ok(new { message = "Propuesta aceptada.", propuesta = actualizada });
        }

        [HttpPut("{id}/rechazar")]
        public async Task<IActionResult> RechazarPropuesta(int id)
        {
            var actualizada = await _propuestaService.ResponderPropuestaAsync(id, aceptada: false);
            if (actualizada == null) return NotFound(new { message = "Propuesta no encontrada." });

            return Ok(new { message = "Propuesta rechazada.", propuesta = actualizada });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exito = await _propuestaService.DeleteAsync(id);
            if (!exito) return NotFound(new { message = "Propuesta no encontrada." });

            return Ok(new { message = "Propuesta eliminada correctamente." });
        }
    }
}