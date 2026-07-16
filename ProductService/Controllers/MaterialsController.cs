using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Entidades;
using Shared.Contracts.Interfaces;



namespace ProductService.Controllers
{
    [Authorize]
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
            var obtenerMat = await _materialRepository.GetAll();
            if (obtenerMat == null)
            {
                return NotFound(new { message = "Materiales no encontrados" });
            }
            return Ok(obtenerMat);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMaterialId(int id)
        {
            var material = await _materialRepository.GetById(id);
            if (material == null)
            {
                return NotFound(new { message = "Material no encontrado" });
            }
            return Ok(material);
        }

        [HttpPost]
        public async Task<IActionResult> CrearMaterial([FromBody] Material crearRequest)
        {
            try
            {
                var crear = await _materialRepository.Create(crearRequest);
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Material dto)
        {
            try
            {
                dto.Id = id; 
                var actualizar = await _materialRepository.Update(dto);
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminar = await _materialRepository.DeleteById(id);
            if (eliminar == null)
            {
                return NotFound(new { message = "Id para eliminar no encontrado" });
            }
            return Ok();
        }


    }
}
