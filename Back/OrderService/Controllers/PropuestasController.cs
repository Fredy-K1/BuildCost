using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Interfaces;
using Shared.Contracts.DTOs.Propuestas;
using System.Security.Claims;

namespace OrderService.Controllers
{
    [Authorize(Roles = "Contratista")]
    [ApiController]
    [Route("api/[controller]")]
    public class PropuestasController : ControllerBase
    {
        private readonly IPropuestaService _propuestaService;

        public PropuestasController(IPropuestaService propuestaService)
        {
            _propuestaService = propuestaService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Token inválido o sin ID de usuario." });

            var contratistaId = Guid.Parse(userIdClaim);
            var propuestas = await _propuestaService.GetByContratistaIdAsync(contratistaId);

            if (!propuestas.Any())
                return NotFound(new { message = "No se encontraron propuestas para este contratista." });

            return Ok(propuestas);
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
                return NotFound(new { message = "Propuesta no encontrada" });
            }
            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePropuestaDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { message = "Token inválido o sin ID de usuario." });

                dto.ContratistaId = Guid.Parse(userIdClaim);

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