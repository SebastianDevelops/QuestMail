using MedPulse.DbContext;
using MedPulse.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedPulse.Repositories;

public class UserMessageRepository : Repository<UserMessage>
{
    public UserMessageRepository(QuestMailContext context) : base(context) { }

    public async Task<List<UserMessage>> GetByUserIdAsync(int? userId)
    {
        if(!userId.HasValue)
            throw new NullReferenceException("UserId cannot be null.");
        
        var chatMessages = await _dbSet.Where(x => x.UserId == userId).ToListAsync();
        
        return chatMessages;
    }

    public async Task<IEnumerable<UserMessage>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<(bool Success, string? Error, UserMessage? Message)> CreateAsync(UserMessage message)
    {
        if (string.IsNullOrWhiteSpace(message.Content))
            return (false, "Content is required.", null);

        await _dbSet.AddAsync(message);
        await _context.SaveChangesAsync();
        return (true, null, message);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(UserMessage message)
    {
        var existing = await _dbSet.FindAsync(message.Id);
        if (existing == null)
            return (false, "UserMessage not found.");

        if (string.IsNullOrWhiteSpace(message.Content))
            return (false, "Content is required.");

        existing.Content = message.Content;
        existing.UserId = message.UserId;

        _dbSet.Update(existing);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var message = await _dbSet.FindAsync(id);
        if (message == null)
            return (false, "UserMessage not found.");

        _dbSet.Remove(message);
        await _context.SaveChangesAsync();
        return (true, null);
    }
}