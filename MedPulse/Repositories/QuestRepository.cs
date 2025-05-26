using MedPulse.DbContext;
    using MedPulse.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    namespace MedPulse.Repositories;
    
    public class QuestRepository : Repository<Quest>
    {
        public QuestRepository(QuestMailContext context) : base(context) { }
    
        public async Task<Quest?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
    
        public async Task<IEnumerable<Quest>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        
        public async Task<List<Quest?>> GetAllByUserIdAsync(int id)
        {
            return await _dbSet.Where(x => x.UserId == id).ToListAsync();
        }
    
        public async Task<(bool Success, string? Error, Quest? Quest)> CreateAsync(Quest quest)
        {
            if (string.IsNullOrWhiteSpace(quest.Title))
                return (false, "Title is required.", null);
    
            await _dbSet.AddAsync(quest);
            await _context.SaveChangesAsync();
            return (true, null, quest);
        }
    
        public async Task<(bool Success, string? Error, Quest? Quest)> UpdateAsync(int Id, string status)
        {
            var existing = await _dbSet.Where(x => x.UserId == Id).FirstOrDefaultAsync();
            if (existing == null)
                return (false, "Quest not found.", existing);
    
            if (string.IsNullOrWhiteSpace(existing.Title))
                return (false, "Title is required.", existing);
    
            if (string.IsNullOrWhiteSpace(existing.Status))
                return (false, "Status is required.", existing);
            
            existing.Status = status;
    
            _dbSet.Update(existing);
            await _context.SaveChangesAsync();
            return (true, null, existing);
        }
    
        public async Task<(bool Success, string? Error)> DeleteAsync(int id)
        {
            var quest = await _dbSet.FindAsync(id);
            if (quest == null)
                return (false, "Quest not found.");
    
            _dbSet.Remove(quest);
            await _context.SaveChangesAsync();
            return (true, null);
        }
    }