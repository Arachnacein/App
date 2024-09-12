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

        public async Task<Pattern> Get(int id)
        {
            return _dbContext.Patterns.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<Pattern>> GetAll()
        {
            return  _dbContext.Patterns; 
        }
        public async Task<Pattern> Add(Pattern pattern)
        {
            _dbContext.Patterns.Add(pattern);
            _dbContext.SaveChanges();
            return pattern;
        }

        public async Task Delete(int id)
        {
            var pattern = _dbContext.Patterns.FirstOrDefault(x => x.Id == id);
            _dbContext.Patterns.Remove(pattern);
            _dbContext.SaveChanges();
        }

    }
    public interface IPatternRepository
    {
        Task<IEnumerable<Pattern>> GetAll();
        Task<Pattern> Get(int id);
        Task<Pattern> Add(Pattern pattern);
        Task Delete(int id);
    }
}
