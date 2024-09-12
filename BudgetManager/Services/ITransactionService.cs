using BudgetManager.Dto.Transaction;

namespace BudgetManager.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> RetrieveTransactions();
        Task<TransactionDto> RetrieveTransaction(int id);
        Task<TransactionDto> AddTransaction(AddTransactionDto transaction);
        Task UpdateTransaction(UpdateTransactionDto transaction);
        Task DeleteTransaction(int id);
        Task UpdateCategory(UpdateTransactionCategoryDto uc);
    }
}
