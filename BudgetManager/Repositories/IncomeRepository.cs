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

        public async Task<Income> Get(int id, Guid userId)
        {
            return _dbContext.Incomes.FirstOrDefault(x => x.Id == id && x.UserId == userId);
        }

        public async Task<IEnumerable<Income>> GetAll(Guid userId)
        {
            return _dbContext.Incomes.Where(x => x.UserId == userId)
                                     .OrderByDescending(x => x.Date);
        }

        public async Task<Income> Add(Income income)
        {
            _dbContext.Incomes.Add(income);
            _dbContext.SaveChanges();
            return income;
        }
        public async Task Update(Income income)
        {
            _dbContext.Incomes.Update(income);
            _dbContext.SaveChanges();
        }

        public async Task Delete(Income income)
        {
            _dbContext.Incomes.Remove(income);
            _dbContext.SaveChanges();
        }

        public async Task<IEnumerable<Income>> Get(MonthYearModel model, Guid userId)
        {
            var incomes = _dbContext.Incomes.Where(x => x.Date.Month == model.Month &&
                                                        x.Date.Year == model.Year &&
                                                        x.UserId == userId);
            return incomes;
        }
    }
    public interface IIncomeRepository
    {
        Task<Income> Get(int id, Guid userId);
        Task<IEnumerable<Income>> Get(MonthYearModel model, Guid userId);
        Task<IEnumerable<Income>> GetAll(Guid userId);
        Task<Income> Add(Income income);
        Task Update(Income income);
        Task Delete(Income income);
    }
}
