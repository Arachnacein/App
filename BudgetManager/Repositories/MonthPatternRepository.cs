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

        public async Task<MonthPattern> Get(int id, Guid userId)
        {
            return _dbCotntext.MonthPatterns.FirstOrDefault(x => x.Id == id && x.UserId == userId);
        }

        public async Task<IEnumerable<MonthPattern>> GetAll(Guid userId)
        {
            return _dbCotntext.MonthPatterns.Where(x => x.UserId == userId);
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

        public async Task<MonthPattern> Get(MonthYearModel model, Guid userId)
        {
            var result = _dbCotntext.MonthPatterns.FirstOrDefault(x => x.Date.Month == model.Month &&
                                                                       x.Date.Year == model.Year &&
                                                                       x.UserId == userId);
            return result;
        }

        public async Task<int> CheckExists(MonthYearModel model, Guid userId)
        {
            var result = _dbCotntext.MonthPatterns.Where(x => x.Date.Month == model.Month &&
                                                              x.Date.Year == model.Year && 
                                                              x.UserId == userId)
                                                  .Count();
            return result;
        }
    }
    public interface IMonthPatternRepository
    {
        Task<MonthPattern> Get(int id, Guid userId);
        Task<MonthPattern> Get(MonthYearModel model, Guid userId);
        Task<int> CheckExists(MonthYearModel model, Guid userId);
        Task<IEnumerable<MonthPattern>> GetAll(Guid userId);
        Task<MonthPattern> Add(MonthPattern pattern);
        Task Update(MonthPattern pattern);
        Task Delete(MonthPattern pattern);
    }
}