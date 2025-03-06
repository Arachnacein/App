using BudgetManager.Dto.Transaction;

namespace BudgetManager.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> RetrieveTransactionsAsync(Guid userId);
        Task<TransactionDto> RetrieveTransactionAsync(int id, Guid userId);
        Task<TransactionDto> AddTransactionAsync(AddTransactionDto transaction);
        Task UpdateTransactionAsync(UpdateTransactionDto transaction);
        Task DeleteTransactionAsync(int id, Guid userId);
        Task UpdateCategoryAsync(UpdateTransactionCategoryDto uc);
        Task ConfirmTransactionAsync(ConfirmTransactionDto dto);
    }
}