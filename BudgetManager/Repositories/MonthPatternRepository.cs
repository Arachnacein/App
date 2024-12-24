using BudgetManager.Data;
using BudgetManager.Dto;
using BudgetManager.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Repositories
{
    public class MonthPatternRepository : IMonthPatternRepository
    {
        private readonly BudgetDbContext _dbCotntext;

        public MonthPatternRepository(BudgetDbContext dbCotntext)
        {
            _dbCotntext = dbCotntext;
        }

        public async Task<MonthPattern> GetAsync(int id, Guid userId)
        {
            return await _dbCotntext.MonthPatterns.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }

        public async Task<IEnumerable<MonthPattern>> GetAllAsync(Guid userId)
        {
            return await _dbCotntext.MonthPatterns.Where(x => x.UserId == userId)
                                                  .ToListAsync();
        }

        public async Task<MonthPattern> AddAsync(MonthPattern pattern)
        {
            await _dbCotntext.MonthPatterns.AddAsync(pattern);
            await _dbCotntext.SaveChangesAsync();
            return pattern;
        }

        public async Task UpdateAsync(MonthPattern pattern)
        {
            _dbCotntext.MonthPatterns.Update(pattern);
            await _dbCotntext.SaveChangesAsync();
        }

        public async Task DeleteAsync(MonthPattern pattern)
        {
            _dbCotntext.MonthPatterns.Remove(pattern);
            await _dbCotntext.SaveChangesAsync();
        }

        public async Task<MonthPattern> GetAsync(MonthYearModel model, Guid userId)
        {
            var result = await _dbCotntext.MonthPatterns.FirstOrDefaultAsync(x => x.Date.Month == model.Month &&
                                                                       x.Date.Year == model.Year &&
                                                                       x.UserId == userId);
            return result;
        }

        public async Task<int> CheckExistsAsync(MonthYearModel model, Guid userId)
        {
            var result = await _dbCotntext.MonthPatterns.Where(x => x.Date.Month == model.Month &&
                                                              x.Date.Year == model.Year && 
                                                              x.UserId == userId)
                                                  .CountAsync();
            return result;
        }
    }
    public interface IMonthPatternRepository
    {
        Task<MonthPattern> GetAsync(int id, Guid userId);
        Task<MonthPattern> GetAsync(MonthYearModel model, Guid userId);
        Task<int> CheckExistsAsync(MonthYearModel model, Guid userId);
        Task<IEnumerable<MonthPattern>> GetAllAsync(Guid userId);
        Task<MonthPattern> AddAsync(MonthPattern pattern);
        Task UpdateAsync(MonthPattern pattern);
        Task DeleteAsync(MonthPattern pattern);
    }
}