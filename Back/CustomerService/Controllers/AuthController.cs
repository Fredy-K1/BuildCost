using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.DTOs.Auth;
using Shared.Contracts.Interfaces;
using System.Security.Claims;

namespace CustomerService.Controllers;

[ApiController]
[Route("api/auth")] 
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;

    public AuthController(IAuthService authService, IUserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await _authService.RegisterAsync(request);
        return Ok(new { Message = "Usuario registrado correctamente" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        return Ok(result);
    }

    [HttpPut("recover")]
    public async Task<IActionResult> Recover([FromBody] RecoverRequest request)
    {
        await _authService.PasswordAsync(request);

        return Ok(new
        {
            Message = "Contraseña restablecida correctamente"
        });
    }

    [HttpGet("obtener")]
    public async Task<IActionResult> ObtenerUsuarios()
    {
        var usuarios = await _authService.GetUsersAsync();

        return Ok(usuarios);
    }

    [HttpPut("actualizar")]
    public async Task<IActionResult> ActualizarPerfil(Guid id, [FromBody] UpdateProfileRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _authService.UpdatePerfilAsync(userId, request);

        return Ok(new
        {
            Message = "Perfil actualizado correctamente"
        });
    }

    [HttpGet("perfil/{id}")]
    public async Task<IActionResult> ObtenerPerfil(Guid id)
    {
        var user = await _authService.PerfilAsync(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }
}
