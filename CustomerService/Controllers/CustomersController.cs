using Microsoft.AspNetCore.Mvc;
using CustomerService.Models;


namespace CustomerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private static List<Registro> customers = new List<Registro>()
        {
            new Registro
            {
                Id       = 1,
                Name     = "Angel",
                Apaterno = "García",
                Amaterno = "López",
                Telefono = "7841000000",
                Email    = "angel@test.com",
                Password = "1234"
            }
        };

        [HttpPost("register")]
        public IActionResult Register([FromBody] Registro newCustomer)
        {
            if (customers.Any(c => c.Email == newCustomer.Email))
                return BadRequest(new { message = "El correo ya está registrado." });

            newCustomer.Id = customers.Count + 1;
            customers.Add(newCustomer);

            return Ok(new { message = "Usuario registrado exitosamente." });
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            var customer = customers.FirstOrDefault(c =>
                c.Email == login.Email &&
                c.Password == login.Password);

            if (customer == null)
                return Unauthorized(new { success = false, message = "Credenciales incorrectas." });

            // Token simulado — en el siguiente paso lo reemplazamos por JWT real
            var fakeToken = Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes($"{customer.Email}:{customer.Id}:{DateTime.UtcNow.Ticks}")
            );

            return Ok(new
            {
                success = true,
                token = fakeToken,
                user = new
                {
                    id = customer.Id,
                    name = customer.Name,
                    email = customer.Email
                }
            });
        }


        [HttpPut("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePassword request)
        {
            var customer = customers.FirstOrDefault(c => c.Email == request.Email);

            if (customer == null)
                return NotFound(new { message = "Usuario no encontrado." });

            customer.Password = request.NewPassword;
            return Ok(new { message = "Contraseña actualizada correctamente." });
        }


        [HttpGet("Obtener_Usuarios")]
        public IActionResult Index()
        {
            return Ok(customers);
        }







    }
}