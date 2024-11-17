using BudgetManager.Dto.Transaction;

namespace BudgetManager.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> RetrieveTransactions(Guid userId);
        Task<TransactionDto> RetrieveTransaction(int id, Guid userId);
        Task<TransactionDto> AddTransaction(AddTransactionDto transaction);
        Task UpdateTransaction(UpdateTransactionDto transaction);
        Task DeleteTransaction(int id, Guid userId);
        Task UpdateCategory(UpdateTransactionCategoryDto uc);
    }
}
