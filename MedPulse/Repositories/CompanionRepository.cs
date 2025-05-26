using MedPulse.DbContext;
using MedPulse.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedPulse.Repositories;

public class CompanionRepository : Repository<Companion>
{
    public CompanionRepository(QuestMailContext context) : base(context) { }

    public async Task<Companion?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<Companion>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<(bool Success, string? Error, Companion? Companion)> CreateAsync(Companion companion)
    {
        if (string.IsNullOrWhiteSpace(companion.Name))
            return (false, "Name is required.", null);

        await _dbSet.AddAsync(companion);
        await _context.SaveChangesAsync();
        return (true, null, companion);
    }

    public async Task<(bool Success, string? Error, Companion companion)> UpdateDescriptionAsync(int id, string description)
    {
        var existing = await _dbSet.FindAsync(id);
        if (existing == null)
            return (false, "Companion not found.", null);
        
        existing.Description = description;

        _dbSet.Update(existing);
        await _context.SaveChangesAsync();
        return (true, null, existing);
    }

    public async Task<(bool Success, string? Error, Companion companion)> UpdateAsync(Companion companion)
    {
        var existing = await _dbSet.FindAsync(companion.Id);
        if (existing == null)
            return (false, "Companion not found.", null);
        
        existing.Name = companion.Name;
        existing.ImageUrl = companion.ImageUrl;
        existing.Description = companion.Description;

        _dbSet.Update(existing);
        await _context.SaveChangesAsync();
        return (true, null, existing);
    }
    
    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var companion = await _dbSet.FindAsync(id);
        if (companion == null)
            return (false, "Companion not found.");

        _dbSet.Remove(companion);
        await _context.SaveChangesAsync();
        return (true, null);
    }
}