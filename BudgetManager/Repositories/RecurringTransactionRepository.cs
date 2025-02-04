using BudgetManager.Data;
using BudgetManager.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Repositories
{
    public class RecurringTransactionRepository : IRecurringTransactionRepository
    {
        private readonly BudgetDbContext _dbContext;
        public RecurringTransactionRepository(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<RecurringTransaction> GetAsync(int id, Guid userId)
        {
            return await _dbContext.RecurringTransactions
                .Include(x => x.Schedule)
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }
        public async Task<IEnumerable<RecurringTransaction>> GetAllAsync(Guid userId)
        {
            return await _dbContext.RecurringTransactions
                .Include(x => x.Schedule)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<RecurringTransaction> AddAsync(RecurringTransaction recurringTransaction)
        {
            _dbContext.Add(recurringTransaction);
            await _dbContext.SaveChangesAsync();
            return recurringTransaction;
        }

        public async Task DeleteAsync(RecurringTransaction recurringTransaction)
        {
            _dbContext.Remove(recurringTransaction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(RecurringTransaction recurringTransaction)
        {
            _dbContext.Update(recurringTransaction);
            await _dbContext.SaveChangesAsync();
        }
    }
    public interface IRecurringTransactionRepository
    {
        Task<RecurringTransaction> GetAsync(int id, Guid userId);
        Task<IEnumerable<RecurringTransaction>> GetAllAsync(Guid userId);
        Task<RecurringTransaction> AddAsync(RecurringTransaction recurringTransaction);
        Task UpdateAsync(RecurringTransaction recurringTransaction);
        Task DeleteAsync(RecurringTransaction recurringTransaction);
    }
}