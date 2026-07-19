using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Data;
using Shared.Contracts.Entities;
using Shared.Contracts.Interfaces;

namespace CustomerService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        email = email.Trim().ToLower();

        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email);
    }
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }
    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
    public async Task<bool> UpdatePerfilAsync(User user)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);
        if (existingUser == null)
            return false;
        existingUser.Name = user.Name;
        existingUser.Apaterno = user.Apaterno;
        existingUser.Amaterno = user.Amaterno;
        existingUser.Telefono = user.Telefono;
        existingUser.Email = user.Email;

        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}