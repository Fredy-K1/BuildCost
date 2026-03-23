using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Models;
using System.Linq;
using CustomerService.Data;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly CustomerDbContext _context;

        public AuthController(CustomerDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            if (_context.Users.Any(u => u.Email == newUser.Email))
            {
                return BadRequest(new { message = "El correo ya está registrado." });
            }

            _context.Users.Add(newUser);
            _context.SaveChanges(); // Guarda  en SQL Server

            return Ok(new { message = "Usuario registrado con éxito", userId = newUser.Id });
        }

        [HttpGet("Obtener")]
        public IActionResult Index()
        {
            var user = _context.Users.FirstOrDefault();

            if (user == null)
                return NotFound(new { message = "No hay usuarios registrados" });

            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Credenciales incorrectas" });
            }

            return Ok(new { message = "Login exitoso", userName = user.Name, email = user.Email });
        }

        [HttpPut("recover")]
        public IActionResult RecoverPassword([FromBody] User recoverInfo)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == recoverInfo.Email);

            if (user == null)
            {
                return NotFound(new { message = "Correo no registrado" });
            }

            user.Password = recoverInfo.Password;
            _context.SaveChanges(); // Actualiza en SQL Server

            return Ok(new { message = "Contraseña actualizada correctamente. Ya puedes iniciar sesión." });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}