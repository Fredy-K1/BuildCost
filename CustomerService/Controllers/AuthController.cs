using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Models;
using System.Collections.Generic;
using System.Linq;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private static List<User> _users = new List<User>();

        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            if (_users.Any(u => u.Email == newUser.Email))
            {
                return BadRequest(new { message = "El correo ya está registrado." });
            }

            newUser.Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(newUser);

            return Ok(new { message = "Usuario registrado con éxito", userId = newUser.Id });
        }

        [HttpGet("Obtener")]
        public IActionResult Index()
        {
            var user = _users.FirstOrDefault();

            if (user == null)
                return NotFound(new { message = "No hay usuarios registrados" });

            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Credenciales incorrectas" });
            }

            return Ok(new { message = "Login exitoso", userName = user.Name, email = user.Email });
        }

        [HttpPut("recover")]
        public IActionResult RecoverPassword([FromBody] User recoverInfo)
        {
            var user = _users.FirstOrDefault(u => u.Email == recoverInfo.Email);

            if (user == null)
            {
                return NotFound(new { message = "Correo no registrado" });
            }

            user.Password = recoverInfo.Password;
            return Ok(new { message = "Contraseña actualizada correctamente. Ya puedes iniciar sesión." });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}