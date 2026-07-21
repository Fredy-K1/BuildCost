using Shared.Contracts.Entidades;

namespace Shared.Contracts.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<User> AddAsync(User user);
    Task<bool> UpdatePerfilAsync(User user);
    Task<List<User>> GetAllAsync();
}