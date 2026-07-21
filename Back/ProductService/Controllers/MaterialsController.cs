using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.DTOs.Product;
using Shared.Contracts.Entidades;
using Shared.Contracts.Interfaces;
using System.Security.Claims;

namespace ProductService.Controllers
{
    [Authorize(Roles = "Contratista")]
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialsController : ControllerBase
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialsController(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMaterials()
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new{message = "Token inválido o sin ID de usuario."});
            }
            var materiales =await _materialRepository.GetAllByContratistaAsync(contratistaId);
            return Ok(materiales);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMaterialId(int id)
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new{message = "Token inválido o sin ID de usuario."});
            }
            var material =await _materialRepository.GetByIdAsync(id,contratistaId);

            if (material == null)
            {
                return NotFound(new{message = "Material no encontrado."});
            }

            return Ok(material);
        }

        [HttpPost]
        public async Task<IActionResult> CrearMaterial([FromBody] MaterialDto dto)
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new{message = "Token inválido o sin ID de usuario."});
            }
            try
            {
                var material = new Material
                {
                    Name = dto.Name,
                    Unit = dto.Unit,
                    Price = dto.Price,
                    ContratistaId = contratistaId
                };

                var creado =await _materialRepository.CreateAsync(material);

                return CreatedAtAction(
                    nameof(GetMaterialId),
                    new { id = creado.Id },
                    creado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new{message = ex.Message});
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,[FromBody] MaterialDto dto)
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new{ message = "Token inválido o sin ID de usuario."});
            }

            try
            {
                var material = new Material
                {
                    Name = dto.Name,
                    Unit = dto.Unit,
                    Price = dto.Price
                };

                var actualizado =await _materialRepository.UpdateAsync(id,contratistaId,material);

                if (actualizado == null)
                {
                    return NotFound(new{ message = "Material no encontrado."});
                }

                return Ok(actualizado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new{message = ex.Message});
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new{message = "Token inválido o sin ID de usuario."});
            }

            var eliminado =await _materialRepository.DeleteAsync(id,contratistaId);

            if (!eliminado)
            {
                return NotFound(new{message = "Material no encontrado."});
            }
            return Ok(new{message = "Material eliminado correctamente."});
        }

        private bool TryGetContratistaId(out Guid contratistaId)
        {
            var claim =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(claim, out contratistaId);
        }
    }
}