using BudgetManager.Data;
using BudgetManager.Dto.Transaction;
using BudgetManager.Models;

namespace BudgetManager.Repositories
{
    public class TransactionRepository : ITransactionRespository
    {
        private readonly BudgetDbContext _context;

        public TransactionRepository(BudgetDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync(Guid userId)
        {
            return _context.Transactions.Where(x => x.UserId == userId);
        }

        public async Task<Transaction> GetAsync(int id, Guid userId)
        {
            return _context.Transactions.FirstOrDefault(t => t.Id == id && t.UserId == userId);
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return transaction;
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var transaction = _context.Transactions.FirstOrDefault(x => x.Id == id);
            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
        }
        public async Task UpdateCategoryAsync(UpdateTransactionCategoryDto uc)
        {
            var transaction = _context.Transactions.FirstOrDefault(x => x.Id == uc.Id);
            transaction.Category = uc.Category;
            _context.Update(transaction);
            _context.SaveChanges();
        }
    }

    public interface ITransactionRespository
    {
        Task<IEnumerable<Transaction>> GetAllAsync(Guid userId);
        Task<Transaction> GetAsync(int id, Guid userId);
        Task<Transaction> AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(int id);
        Task UpdateCategoryAsync(UpdateTransactionCategoryDto uc);
    }
}