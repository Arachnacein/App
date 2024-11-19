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

        public async Task<Pattern> Get(int id, Guid userId)
        {
            return _dbContext.Patterns.FirstOrDefault(x => x.Id == id && x.UserId == userId);
        }

        public async Task<IEnumerable<Pattern>> GetAll(Guid userId)
        {
            return  _dbContext.Patterns.Where(x => x.UserId == userId); 
        }
        public async Task<Pattern> Add(Pattern pattern)
        {
            _dbContext.Patterns.Add(pattern);
            _dbContext.SaveChanges();
            return pattern;
        }

        public async Task Delete(int id, Guid userId)
        {
            var pattern = _dbContext.Patterns.FirstOrDefault(x => x.Id == id && x.UserId == userId);
            _dbContext.Patterns.Remove(pattern);
            _dbContext.SaveChanges();
        }

    }
    public interface IPatternRepository
    {
        Task<IEnumerable<Pattern>> GetAll(Guid userId);
        Task<Pattern> Get(int id, Guid userId);
        Task<Pattern> Add(Pattern pattern);
        Task Delete(int id, Guid userId);
    }
}
