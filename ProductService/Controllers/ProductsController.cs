using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Pegado de block m²",     Price = 150 },
            new Product { Id = 2, Name = "Piso m²",                Price = 200 },
            new Product { Id = 3, Name = "Zapata m²",              Price = 350 },
            new Product { Id = 4, Name = "Losa primera planta m²", Price = 450 },
            new Product { Id = 5, Name = "Cadena m²",              Price = 280 },
        };

        [HttpGet("ObtenerProductos")]
        public IActionResult GetAll() => Ok(_products);

        [HttpPost("GuardarProducto")]
        public IActionResult Create([FromBody] Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                return BadRequest(new { message = "El nombre es requerido." });

            product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
            return Ok(new { message = "Material agregado.", product });
        }

        [HttpPut("EditarProducto")]
        public IActionResult Update([FromBody] Product updated)
        {
            var product = _products.FirstOrDefault(p => p.Id == updated.Id);
            if (product == null)
                return NotFound(new { message = "Material no encontrado." });

            product.Name = updated.Name;
            product.Price = updated.Price;
            return Ok(new { message = "Material actualizado.", product });
        }

        [HttpDelete("EliminarProducto")]
        public IActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound(new { message = "Material no encontrado." });

            _products.Remove(product);
            return Ok(new { message = "Material eliminado." });
        }
    }
}