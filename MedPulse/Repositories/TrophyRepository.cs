using MedPulse.DbContext;
using MedPulse.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedPulse.Repositories;

public class TrophyRepository : Repository<Trophy>
{
    public TrophyRepository(QuestMailContext context) : base(context) { }

    public async Task<Trophy?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<Trophy>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
    
    public async Task<List<Trophy>?> GetAllByUserIdAsync(int Id)
    {
        return await _dbSet.Where(x => x.UserId == Id).ToListAsync();
    }

    public async Task<(bool Success, string? Error, Trophy? Trophy)> CreateAsync(Trophy trophy)
    {
        if (string.IsNullOrWhiteSpace(trophy.Name))
            return (false, "Name is required.", null);

        await _dbSet.AddAsync(trophy);
        await _context.SaveChangesAsync();
        return (true, null, trophy);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Trophy trophy)
    {
        var existing = await _dbSet.FindAsync(trophy.Id);
        if (existing == null)
            return (false, "Trophy not found.");

        if (string.IsNullOrWhiteSpace(trophy.Name))
            return (false, "Name is required.");

        existing.Name = trophy.Name;
        existing.Description = trophy.Description;
        existing.ImageUrl = trophy.ImageUrl;
        existing.UserId = trophy.UserId;

        _dbSet.Update(existing);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var trophy = await _dbSet.FindAsync(id);
        if (trophy == null)
            return (false, "Trophy not found.");

        _dbSet.Remove(trophy);
        await _context.SaveChangesAsync();
        return (true, null);
    }
}