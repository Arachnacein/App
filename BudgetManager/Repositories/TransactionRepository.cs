using BudgetManager.Data;
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

        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transactions;
        }

        public Transaction Get(int id)
        {
            return _context.Transactions.FirstOrDefault(t => t.Id == id);
        }

        public Transaction Add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return transaction;
        }

        public void Update(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var transaction = _context.Transactions.FirstOrDefault(x => x.Id == id);
            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
        }
    }

    public interface ITransactionRespository
    {
        IEnumerable<Transaction> GetAll();
        Transaction Get(int id);
        Transaction Add(Transaction transaction);
        void Update(Transaction transaction);
        void Delete(int id);
    }
}
