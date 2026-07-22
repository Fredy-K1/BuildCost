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
    public class DatosController : ControllerBase
    {
        private readonly IDatoM2Repository _datosRepository;

        public DatosController(
            IDatoM2Repository datosRepository)
        {
            _datosRepository = datosRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDatosM2()
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new{message = "Token inválido o sin ID de usuario."});
            }

            var datos =await _datosRepository.GetAllByContratistaAsync(contratistaId);
            return Ok(datos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDatosM2Id(int id)
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new{message = "Token inválido o sin ID de usuario."});
            }

            var datoM2 =await _datosRepository.GetByIdAsync(id,contratistaId);
            if (datoM2 == null)
            {
                return NotFound(new{message = "Dato M2 no encontrado."});
            }

            return Ok(datoM2);
        }

        [HttpPost]
        public async Task<IActionResult> CrearDatoM2([FromBody] DatoM2Dto dto)
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new{message = "Token inválido o sin ID de usuario."});
            }

            try
            {
                var datoM2 = new DatosM2
                {
                    DataType = dto.DataType,
                    Value = dto.Value,
                    Description = dto.Description,
                    ContratistaId = contratistaId
                };

                var creado =await _datosRepository.CreateAsync(datoM2);

                return CreatedAtAction(nameof(GetDatosM2Id),new { id = creado.Id },creado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new{ message = ex.Message});
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,[FromBody] DatoM2Dto dto)
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new{message = "Token inválido o sin ID de usuario."});
            }

            try
            {
                var datoM2 = new DatosM2
                {
                    DataType = dto.DataType,
                    Value = dto.Value,
                    Description = dto.Description
                };

                var actualizado =await _datosRepository.UpdateAsync(id,contratistaId,datoM2);

                if (actualizado == null)
                {
                    return NotFound(new
                    {message = "Dato M2 no encontrado."});
                }

                return Ok(actualizado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new{ message = ex.Message});
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new
                {message = "Token inválido o sin ID de usuario."});
            }

            var eliminado =await _datosRepository.DeleteAsync(id,contratistaId);

            if (!eliminado)
            {
                return NotFound(new{message = "Dato M2 no encontrado."});
            }

            return Ok(new{message = "Dato M2 eliminado correctamente."});
        }

        private bool TryGetContratistaId(out Guid contratistaId)
        {
            var claim =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(claim, out contratistaId);
        }
    }
}