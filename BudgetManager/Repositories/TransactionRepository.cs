using BudgetManager.Data;
using BudgetManager.Dto;
using BudgetManager.Models;

namespace BudgetManager.Repositories
{
    public class TransactionRepository : ITransactionRespository
    {
        private readonly TransactionDbContext _context;

        public TransactionRepository(TransactionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetAll()
        {
            return _context.Transactions;
        }

        public async Task<Transaction> Get(int id)
        {
            return _context.Transactions.FirstOrDefault(t => t.Id == id);
        }

        public async Task<Transaction> Add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return transaction;
        }

        public async Task Update(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var transaction = _context.Transactions.FirstOrDefault(x => x.Id == id);
            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
        }

        public async Task UpdateCategory(UpdateTransactionCategoryDto uc)
        {
            var transaction = _context.Transactions.FirstOrDefault(x => x.Id == uc.Id);
            transaction.Category = uc.Category;
            _context.SaveChanges();
        }
    }

    public interface ITransactionRespository
    {
        Task<IEnumerable<Transaction>> GetAll();
        Task<Transaction> Get(int id);
        Task<Transaction> Add(Transaction transaction);
        Task Update(Transaction transaction);
        Task Delete(int id);

        Task UpdateCategory(UpdateTransactionCategoryDto uc);
    }
}
