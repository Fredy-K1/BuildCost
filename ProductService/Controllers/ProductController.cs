using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using Microsoft.AspNetCore.Authorization; 

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public ProductController(ProductDbContext context)
        {
            _context = context;
        }

        [Authorize] // Solo usuarios autenticados pueden acceder a este endpoint
        [HttpGet("obtener")]
        public async Task<IActionResult> GetProducts()
        {
            // Va a la base de datos, trae los productos y los devuelve en formato JSON
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }
    }
}