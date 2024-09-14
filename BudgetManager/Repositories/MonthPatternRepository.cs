using BudgetManager.Data;
using BudgetManager.Dto;
using BudgetManager.Models;

namespace BudgetManager.Repositories
{
    public class MonthPatternRepository : IMonthPatternRepository
    {
        private readonly BudgetDbContext _dbCotntext;

        public MonthPatternRepository(BudgetDbContext dbCotntext)
        {
            _dbCotntext = dbCotntext;
        }

        public async Task<MonthPattern> Get(int id)
        {
            return _dbCotntext.MonthPatterns.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<MonthPattern>> GetAll()
        {
            return _dbCotntext.MonthPatterns;
        }

        public async Task<MonthPattern> Add(MonthPattern pattern)
        {
            _dbCotntext.MonthPatterns.Add(pattern);
            _dbCotntext.SaveChanges();
            return pattern;
        }
        public async Task Update(MonthPattern pattern)
        {
            _dbCotntext.MonthPatterns.Update(pattern);
            _dbCotntext.SaveChanges();
        }

        public async Task Delete(MonthPattern pattern)
        {
            _dbCotntext.MonthPatterns.Remove(pattern);
            _dbCotntext.SaveChanges();
        }

        public async Task<MonthPattern> Get(MonthYearModel model)
        {
            var result = _dbCotntext.MonthPatterns.FirstOrDefault(x => x.Date.Month == model.Month && x.Date.Year == model.Year);
            return result;
        }
    }
    public interface IMonthPatternRepository
    {
        Task<MonthPattern> Get(int id);
        Task<MonthPattern> Get(MonthYearModel model);
        Task<IEnumerable<MonthPattern>> GetAll();
        Task<MonthPattern> Add(MonthPattern pattern);
        Task Update(MonthPattern pattern);
        Task Delete(MonthPattern pattern);
    }
}