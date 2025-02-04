using BudgetManager.Dto.RecurringTransaction;
using BudgetManager.Repositories;

namespace BudgetManager.Services
{
    public class RecurringTransactionService : IRecurringTransactionService
    {
        private readonly IRecurringTransactionRepository _recurringTransactionRepository;

        public RecurringTransactionService(IRecurringTransactionRepository repository)
        {
            _recurringTransactionRepository = repository;
        }

        public Task<RecurringTransactionDto> AddAsync(AddRecurringTransactionDto recurringTransaction)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RecurringTransactionDto>> GetAllAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<RecurringTransactionDto> GetAsync(int id, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UpdateRecurringTransactionDto recurringTransaction)
        {
            throw new NotImplementedException();
        }
    }
}