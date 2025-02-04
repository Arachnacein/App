using BudgetManager.Dto.RecurringTransaction;
using BudgetManager.Exceptions;
using BudgetManager.Exceptions.TransactionExceptions;
using BudgetManager.Mappers;
using BudgetManager.Repositories;

namespace BudgetManager.Services
{
    public class RecurringTransactionService : IRecurringTransactionService
    {
        private readonly IRecurringTransactionRepository _recurringTransactionRepository;
        private readonly IRecurringTransactionMapper _recurringTransactionMapper;

        public RecurringTransactionService(IRecurringTransactionRepository repository, IRecurringTransactionMapper recurringTransactionMapper)
        {
            _recurringTransactionRepository = repository;
            _recurringTransactionMapper = recurringTransactionMapper;
        }

        public async Task<RecurringTransactionDto> GetAsync(int id, Guid userId)
        {
            var recurringTransaction = await _recurringTransactionRepository.GetAsync(id, userId);
            if(recurringTransaction == null)
                throw new RecurringTransactionNotFoundException($"Recurring transaction not found. Id:{id}");
            return _recurringTransactionMapper.Map(recurringTransaction);
        }

        public async Task<IEnumerable<RecurringTransactionDto>> GetAllAsync(Guid userId)
        {
            var recurringTransactions = await _recurringTransactionRepository.GetAllAsync(userId);
            return _recurringTransactionMapper.MapElements(recurringTransactions.ToList());
        }

        public async Task<RecurringTransactionDto> AddAsync(AddRecurringTransactionDto recurringTransaction)
        {
            if(recurringTransaction == null)
                throw new ArgumentNullException("Object is null");
            if (recurringTransaction.Name.Length <= 3)
                throw new BadStringLengthException($"Name have incorrect length. Should be more than 3 characters.");
            if (recurringTransaction.Name.Length >= 50)
                throw new BadStringLengthException($"Name have incorrect length. Should be less than 50 characters.");

            var mappedRecurringTransaction = _recurringTransactionMapper.Map(recurringTransaction);
            await _recurringTransactionRepository.AddAsync(mappedRecurringTransaction);
            return _recurringTransactionMapper.Map(mappedRecurringTransaction);
        }
        public async Task UpdateAsync(UpdateRecurringTransactionDto recurringTransaction)
        {
            if (recurringTransaction == null)
                throw new ArgumentNullException("Object is null");
            if (recurringTransaction.Name.Length <= 3)
                throw new BadStringLengthException($"Name have incorrect length. Should be more than 3 characters.");
            if (recurringTransaction.Name.Length >= 50)
                throw new BadStringLengthException($"Name have incorrect length. Should be less than 50 characters.");

            var mappedRecurringTransaction = _recurringTransactionMapper.Map(recurringTransaction);
            await _recurringTransactionRepository.UpdateAsync(mappedRecurringTransaction);
        }

        public async Task DeleteAsync(int id, Guid userId)
        {
            var recurringTransaction = await _recurringTransactionRepository.GetAsync(id, userId);
            if (recurringTransaction == null)
                throw new RecurringTransactionNotFoundException($"Recurring transaction not found. Id:{id}");
            await _recurringTransactionRepository.DeleteAsync(recurringTransaction);
        }
    }
}