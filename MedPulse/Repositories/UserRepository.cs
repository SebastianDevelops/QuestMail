using MedPulse.DbContext;
using MedPulse.Models;
using Microsoft.EntityFrameworkCore;

namespace MedPulse.Repositories;

public class UserRepository : Repository<User>
{
    public UserRepository(QuestMailContext context) : base(context) { }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<int?> GetUserIdByEmailAsync(string email)
    {
        var id = await _dbSet.Where(x => String.Equals(x.Email, email)).Select(x => x.Id).FirstOrDefaultAsync();
            return id;
    }
    
    public async Task<(bool Success, string? Error, User? User)> CreateUserAsync(User user)
    {
        var existingUser = await FindByEmailAsync(user.Email);
        if (existingUser != null)
        {
            return (true, null, existingUser);
        }
        if (string.IsNullOrWhiteSpace(user.Email))
            return (false, "Email is required.", null);

        if (existingUser != null)
            return (false, "Email already exists.", null);

        await _dbSet.AddAsync(user);
        await _context.SaveChangesAsync();
        return (true, null, user);
    }
    
    public async Task<(bool Success, string? Error)> UpdateUserAsync(User user)
    {
        var existing = await _dbSet.FindAsync(user.Id);
        if (existing == null)
            return (false, "User not found.");

        if (string.IsNullOrWhiteSpace(user.Email))
            return (false, "Email is required.");

        var emailOwner = await FindByEmailAsync(user.Email);
        if (emailOwner != null && emailOwner.Id != user.Id)
            return (false, "Email already exists.");

        existing.Email = user.Email;
        existing.Name = user.Name;
        existing.FullName = user.FullName;

        _dbSet.Update(existing);
        await _context.SaveChangesAsync();
        return (true, null);
    }
}