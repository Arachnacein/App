using BudgetManager.Data;
using BudgetManager.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Repositories
{
    public class PatternRepository : IPatternRepository
    {
        private readonly BudgetDbContext _dbContext;

        public PatternRepository(BudgetDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Pattern> GetAsync(int id, Guid userId)
        {
            return await _dbContext.Patterns.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }

        public async Task<IEnumerable<Pattern>> GetAllAsync(Guid userId)
        {
            return  await _dbContext.Patterns.Where(x => x.UserId == userId)
                .ToListAsync(); 
        }
        public async Task<Pattern> AddAsync(Pattern pattern)
        {
            await _dbContext.Patterns.AddAsync(pattern);
            await _dbContext.SaveChangesAsync();
            return pattern;
        }

        public async Task DeleteAsync(int id, Guid userId)
        {
            var pattern = await _dbContext.Patterns.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            _dbContext.Patterns.Remove(pattern);
            await _dbContext.SaveChangesAsync();
        }

    }
    public interface IPatternRepository
    {
        Task<IEnumerable<Pattern>> GetAllAsync(Guid userId);
        Task<Pattern> GetAsync(int id, Guid userId);
        Task<Pattern> AddAsync(Pattern pattern);
        Task DeleteAsync(int id, Guid userId);
    }
}