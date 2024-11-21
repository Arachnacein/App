using BudgetManager.Data;
using BudgetManager.Dto;
using BudgetManager.Models;

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
            return _dbContext.Incomes.FirstOrDefault(x => x.Id == id && x.UserId == userId);
        }

        public async Task<IEnumerable<Income>> GetAllAsync(Guid userId)
        {
            return _dbContext.Incomes.Where(x => x.UserId == userId)
                                     .OrderByDescending(x => x.Date);
        }

        public async Task<Income> AddAsync(Income income)
        {
            _dbContext.Incomes.Add(income);
            _dbContext.SaveChanges();
            return income;
        }
        public async Task UpdateAsync(Income income)
        {
            _dbContext.Incomes.Update(income);
            _dbContext.SaveChanges();
        }

        public async Task DeleteAsync(Income income)
        {
            _dbContext.Incomes.Remove(income);
            _dbContext.SaveChanges();
        }

        public async Task<IEnumerable<Income>> GetAsync(MonthYearModel model, Guid userId)
        {
            var incomes = _dbContext.Incomes.Where(x => x.Date.Month == model.Month &&
                                                        x.Date.Year == model.Year &&
                                                        x.UserId == userId);
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