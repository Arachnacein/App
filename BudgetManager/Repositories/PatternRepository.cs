using BudgetManager.Data;
using BudgetManager.Models;

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
            return _dbContext.Patterns.FirstOrDefault(x => x.Id == id && x.UserId == userId);
        }

        public async Task<IEnumerable<Pattern>> GetAllAsync(Guid userId)
        {
            return  _dbContext.Patterns.Where(x => x.UserId == userId); 
        }
        public async Task<Pattern> AddAsync(Pattern pattern)
        {
            _dbContext.Patterns.Add(pattern);
            _dbContext.SaveChanges();
            return pattern;
        }

        public async Task DeleteAsync(int id, Guid userId)
        {
            var pattern = _dbContext.Patterns.FirstOrDefault(x => x.Id == id && x.UserId == userId);
            _dbContext.Patterns.Remove(pattern);
            _dbContext.SaveChanges();
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