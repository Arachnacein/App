using BudgetManager.Dto.RecurringTransaction;

namespace BudgetManager.Services
{
    public interface IRecurringTransactionService
    {
        Task<RecurringTransactionDto> RetrieveRecurringTransactionAsync(int id, Guid userId);
        Task<IEnumerable<RecurringTransactionDto>> RetrieveRecurringTransactionsAsync(Guid userId);
        Task<RecurringTransactionDto> AddRecurringTransactionAsync(AddRecurringTransactionDto recurringTransaction);
        Task<RecurringTransactionDto> AddCustomRecurringTransactionAsync(AddRecurringTransactionDto recurringTransaction);
        Task UpdateRecurringTransactionAsync(UpdateRecurringTransactionDto recurringTransaction);
        Task DeleteRecurringTransactionAsync(int id, Guid userId);
    }
}