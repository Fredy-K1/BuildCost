using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Entidades;
using Shared.Contracts.Interfaces;



namespace ProductService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository repository)
        {
            _productRepository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var obtenerProd = await _productRepository.GetAll();
            if (obtenerProd == null)
            {
                return NotFound(new { message = "Productos no encontrados" });
            }
            return Ok(obtenerProd);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductId(int id)
        {
            var producto = await _productRepository.GetById(id);
            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }
            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] Product crearRequest)
        {
            try
            {
                var crear = await _productRepository.Create(crearRequest);
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
        public async Task<IActionResult> Update(int id, [FromBody] Product dto)
        {
            try
            {
                var actualizar = await _productRepository.UpdateById(id, dto);
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
            var eliminar = await _productRepository.Delete(id);
            if (eliminar == false)
            {
                return NotFound(new { message = "Id para eliminar no encontrado" });
            }
            return Ok();
        }

    }
}
