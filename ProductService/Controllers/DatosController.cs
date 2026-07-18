using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Entidades;
using Shared.Contracts.Interfaces;

namespace ProductService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DatosController : ControllerBase
    {
        private readonly IDatoM2Repository _datosRepository;

        public DatosController(IDatoM2Repository datosRepository)
        {
            _datosRepository = datosRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDatosM2()
        {
            var obtenerDat = await _datosRepository.GetAll();
            if (obtenerDat == null)
            {
                return NotFound(new { message = "Datos M2 no encontrados" });
            }
            return Ok(obtenerDat);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetDatosM2Id(Guid id)
        {
            var datoM2 = await _datosRepository.GetById(id);
            if (datoM2 == null)
            {
                return NotFound(new { message = "Dato M2 no encontrado" });
            }
            return Ok(datoM2);
        }

        [HttpPost]
        public async Task<IActionResult> CrearDatoM2([FromBody] DatosM2 crearRequest)
        {
            try
            {
                var crear = await _datosRepository.Create(crearRequest);
                if (crear == null)
                {
                    return BadRequest(new { message = "Datos incompletos" });
                }
                return Ok(crear);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DatosM2 dto)
        {
            try
            {
                var actualizar = await _datosRepository.Update(id, dto);
                if (actualizar == null)
                {
                    return NotFound(new { message = "Id para actualizar no encontrado" });
                }
                return Ok(actualizar);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var eliminar = await _datosRepository.Delete(id);
            if (eliminar == false)
            {
                return NotFound(new { message = "Id para eliminar no encontrado" });
            }
            return Ok();
        }





    }
}
