using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Interfaces;
using Shared.Contracts.DTOs.Projec;
using Shared.Contracts.Enums;
using System.Security.Claims;

namespace OrderService.Controllers
{
    [Authorize(Roles = "Cliente")]
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Token inválido o sin ID de usuario." });

            var userId = Guid.Parse(userIdClaim);
            var proyectos = await _proyectoService.GetAllProyectosAsync(userId);

            if (!proyectos.Any())
                return NotFound(new { message = "No se encontraron proyectos para este usuario." });

            return Ok(proyectos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Token inválido o sin ID de usuario." });

            var userId = Guid.Parse(userIdClaim);
            var proyecto = await _proyectoService.GetProyectoByIdAsync(id, userId);

            if (proyecto == null)
                return NotFound(new { message = "Proyecto no encontrado o no pertenece al usuario." });

            return Ok(proyecto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProyectoDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Token inválido o sin ID de usuario." });

            dto.UserOwnerId = Guid.Parse(userIdClaim);
            var proyecto = await _proyectoService.CreateAsync(dto);
            return StatusCode(201, proyecto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProyectoDto dto)
        {
            var actualizado = await _proyectoService.UpdateAsync(id, dto);
            if (actualizado == null)
                return NotFound(new { message = "Proyecto no encontrado" });

            return Ok(actualizado);
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] ProjectStatus estado)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Token inválido o sin ID de usuario." });

            var userId = Guid.Parse(userIdClaim);
            var actualizado = await _proyectoService.CambiarEstadoAsync(id, estado, userId);

            if (actualizado == null)
                return NotFound(new { message = "Proyecto no encontrado o no pertenece al usuario." });

            return Ok(new { message = "Estado actualizado", proyecto = actualizado });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _proyectoService.DeleteAsync(id);
            if (eliminado == null)
                return NotFound(new { message = "Proyecto no encontrado" });

            return Ok(new { message = "Proyecto eliminado con éxito" });
        }
    }
}
