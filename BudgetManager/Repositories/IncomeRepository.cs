using BudgetManager.Data;
using BudgetManager.Dto;
using BudgetManager.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Repositories
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly BudgetDbContext _dbContext;

        public IncomeRepository(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Income> GetAsync(int id, Guid userId)
        {
            return await _dbContext.Incomes
                      .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }

        public async Task<IEnumerable<Income>> GetAllAsync(Guid userId)
        {
            return await _dbContext.Incomes.Where(x => x.UserId == userId)
                                           .OrderByDescending(x => x.Date)
                                           .ToListAsync();
        }

        public async Task<Income> AddAsync(Income income)
        {
            await _dbContext.Incomes.AddAsync(income);
            await _dbContext.SaveChangesAsync();
            return income;
        }
        public async Task UpdateAsync(Income income)
        {
            _dbContext.Incomes.Update(income);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Income income)
        {
             _dbContext.Incomes.Remove(income);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Income>> GetAsync(MonthYearModel model, Guid userId)
        {
            var incomes = await _dbContext.Incomes
                                    .Where(x => x.Date.Month == model.Month &&
                                                x.Date.Year == model.Year &&
                                                x.UserId == userId)
                                     .ToListAsync();
            return incomes;
        }
    }
    public interface IIncomeRepository
    {
        Task<Income> GetAsync(int id, Guid userId);
        Task<IEnumerable<Income>> GetAsync(MonthYearModel model, Guid userId);
        Task<IEnumerable<Income>> GetAllAsync(Guid userId);
        Task<Income> AddAsync(Income income);
        Task UpdateAsync(Income income);
        Task DeleteAsync(Income income);
    }
}