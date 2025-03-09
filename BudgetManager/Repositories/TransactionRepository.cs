using BudgetManager.Data;
using BudgetManager.Dto.Transaction;
using BudgetManager.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BudgetDbContext _context;

        public TransactionRepository(BudgetDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync(Guid userId)
        {
            return await _context.Transactions.Where(x => x.UserId == userId)
                                              .ToListAsync();
        }

        public async Task<Transaction> GetAsync(int id, Guid userId)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCategoryAsync(UpdateTransactionCategoryDto uc)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == uc.Id);
            transaction.Category = uc.Category;
            _context.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task ConfirmTransactionAsync(ConfirmTransactionDto dto)
        {
            var currentTransaction = _context.Transactions
                .FirstOrDefault(x => x.Id == dto.Id && x.UserId == dto.UserId);
            currentTransaction.IsApproved = true;
            _context.Update(currentTransaction);
            await _context.SaveChangesAsync();
        }
    }

    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllAsync(Guid userId);
        Task<Transaction> GetAsync(int id, Guid userId);
        Task<Transaction> AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(int id);
        Task UpdateCategoryAsync(UpdateTransactionCategoryDto dto);
        Task ConfirmTransactionAsync(ConfirmTransactionDto dto);
    }
}