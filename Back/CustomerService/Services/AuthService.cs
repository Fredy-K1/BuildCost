using Shared.Contracts.DTOs.Auth;
using Shared.Contracts.Entidades;
using Shared.Contracts.Interfaces;
using Shared.Contracts.Services;

namespace CustomerService.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }
    public async Task RegisterAsync(RegisterRequest request)
    {
        var user = new User
        {
            Name = request.Name,
            Apaterno = request.Apaterno,
            Amaterno = request.Amaterno,
            Telefono = request.Telefono,
            Email = request.Email,
            Password = EncryptionService.HashSHA256(request.Password),
            Role = request.Role,
        };

        await _userRepository.AddAsync(user);
    }
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
            throw new Exception("Usuario no encontrado.");

        var passwordHash = EncryptionService.HashSHA256(request.Password);

        if (user.Password != passwordHash)
            throw new Exception("Contraseña incorrecta.");

        var key = _configuration["Jwt:Key"]!;

        var token = JwtService.GenerateToken(user, key);

        return new LoginResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Token = token
        };
    }
    public async Task PasswordAsync(RecoverRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
            throw new Exception("Usuario no encontrado.");

        user.Password = EncryptionService.HashSHA256(request.Password);
        await _userRepository.UpdatePerfilAsync(user);
    }
    public async Task<List<User>> GetUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }
    public async Task<User?> PerfilAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task UpdatePerfilAsync(Guid userId, UpdateProfileRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            throw new Exception("Usuario no encontrado.");
        user.Name = request.Name;
        user.Apaterno = request.Apaterno;
        user.Amaterno = request.Amaterno;
        user.Telefono = request.Telefono;
        await _userRepository.UpdatePerfilAsync(user);
    }
}