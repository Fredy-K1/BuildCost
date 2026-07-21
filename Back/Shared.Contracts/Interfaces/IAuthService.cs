using Shared.Contracts.DTOs.Auth;
using Shared.Contracts.Entidades;

namespace Shared.Contracts.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task PasswordAsync(RecoverRequest request);
    Task<List<User>> GetUsersAsync();
    Task<User?> PerfilAsync(Guid id);
    Task UpdatePerfilAsync(Guid userId, UpdateProfileRequest request);
}