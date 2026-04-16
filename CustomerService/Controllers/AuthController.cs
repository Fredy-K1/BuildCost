using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Models;
using System.Linq;
using CustomerService.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly CustomerDbContext _context;
        private readonly IConfiguration _config; 

        public AuthController(CustomerDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            if (_context.Users.Any(u => u.Email == newUser.Email))
            {
                return BadRequest(new { message = "El correo ya está registrado." });
            }

            // Encripta la contraseña antes de guardar
            newUser.Password = EncriptarPassword(newUser.Password);

            _context.Users.Add(newUser);
            _context.SaveChanges(); // Guarda en SQL Server

            return Ok(new { message = "Usuario registrado con éxito", userId = newUser.Id });
        }

        [Authorize]
        [HttpGet("Obtener")]
        public IActionResult Index()
        {
            var user = _context.Users.FirstOrDefault();

            if (user == null)
                return NotFound(new { message = "No hay usuarios registrados" });

            // DTO Aplicado 
            var respuesta = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone
            };

            return Ok(respuesta);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Encripta la contraseña ingresada para comparar
            string passwordEncriptada = EncriptarPassword(request.Password);
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email && u.Password == passwordEncriptada);

            if (user == null)
            {
                return Unauthorized(new { message = "Credenciales incorrectas" });
            }

            // GENERAMOS EL TOKEN JWT
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Estos son los datos que irán EMPAQUETADOS dentro del Token 
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim("nombre", user.Name),
                new Claim("telefono", user.Phone)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2), 
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                message = "Login exitoso",
                token = tokenString,
                userName = user.Name,
                email = user.Email,
                phone = user.Phone
            });
        }

        [HttpPut("recover")]
        public IActionResult RecoverPassword([FromBody] User recoverInfo)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == recoverInfo.Email);

            if (user == null)
            {
                return NotFound(new { message = "Correo no registrado" });
            }

            // Encriptamos la nueva contraseña antes de actualizarla
            user.Password = EncriptarPassword(recoverInfo.Password);

            _context.SaveChanges(); // Actualiza en SQL Server

            return Ok(new { message = "Contraseña actualizada correctamente. Ya puedes iniciar sesión." });
        }

        private string EncriptarPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder constructor = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    constructor.Append(bytes[i].ToString("x2"));
                }
                return constructor.ToString();
            }
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}