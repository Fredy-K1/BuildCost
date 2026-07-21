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
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new{message = "Token inválido o sin ID de usuario."});
            }

            var productos =await _productRepository.GetAllByContratistaAsync(contratistaId);

            return Ok(productos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductId(int id)
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new{message = "Token inválido o sin ID de usuario."});
            }

            var producto =await _productRepository.GetByIdAsync(id,contratistaId);

            if (producto == null)
            {
                return NotFound(new{message = "Producto no encontrado."});
            }

            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] ProductDto dto)
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new{message = "Token inválido o sin ID de usuario."});
            }

            try
            {
                var producto = new Product
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    ContratistaId = contratistaId
                };

                var creado =await _productRepository.CreateAsync(producto);

                return CreatedAtAction(nameof(GetProductId),new { id = creado.Id },creado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new{ message = ex.Message});
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,[FromBody] ProductDto dto)
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new{message = "Token inválido o sin ID de usuario."});
            }

            try
            {
                var producto = new Product
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price
                };

                var actualizado =await _productRepository.UpdateAsync(id,contratistaId,producto);

                if (actualizado == null)
                {
                    return NotFound(new{message = "Producto no encontrado."});
                }

                return Ok(actualizado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new{ message = ex.Message});}
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!TryGetContratistaId(out var contratistaId))
            {
                return Unauthorized(new{message = "Token inválido o sin ID de usuario."});
            }

            var eliminado =await _productRepository.DeleteAsync(id,contratistaId
                );

            if (!eliminado)
            {
                return NotFound(new{message = "Producto no encontrado."});
            }

            return Ok(new{message = "Producto eliminado correctamente."});
        }

        private bool TryGetContratistaId(out Guid contratistaId)
        {
            var claim =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(claim, out contratistaId);
        }
    }
}