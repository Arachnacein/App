using BudgetManager.Dto.RecurringTransaction;

namespace BudgetManager.Services
{
    public interface IRecurringTransactionService
    {
        Task<RecurringTransactionDto> GetAsync(int id, Guid userId);
        Task<IEnumerable<RecurringTransactionDto>> GetAllAsync(Guid userId);
        Task<RecurringTransactionDto> AddAsync(AddRecurringTransactionDto recurringTransaction);
        Task UpdateAsync(UpdateRecurringTransactionDto recurringTransaction);
        Task DeleteAsync(int id, Guid userId);
    }
}