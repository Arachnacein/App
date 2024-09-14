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

        public async Task<Income> Get(int id)
        {
            return _dbContext.Incomes.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<Income>> GetAll()
        {
            return _dbContext.Incomes;
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

        public async Task<IEnumerable<Income>> Get(MonthYearModel model)
        {
            var incomes = _dbContext.Incomes.Where(x => x.Date.Month == model.Month &
                                                        x.Date.Year == model.Year);
            return incomes;
        }
    }
    public interface IIncomeRepository
    {
        Task<Income> Get(int id);
        Task<IEnumerable<Income>> Get(MonthYearModel model);
        Task<IEnumerable<Income>> GetAll();
        Task<Income> Add(Income income);
        Task Update(Income income);
        Task Delete(Income income);
    }
}
