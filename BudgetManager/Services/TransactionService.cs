using BudgetManager.Dto.Transaction;
using BudgetManager.Exceptions;
using BudgetManager.Exceptions.TransactionExceptions;
using BudgetManager.Mappers;
using BudgetManager.Models;
using BudgetManager.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace BudgetManager.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRespository _repository;
        private readonly ITransactionMapper _transactionMapper;

        public TransactionService(ITransactionRespository transactionRepository, ITransactionMapper transactionMapper)
        {
            _repository = transactionRepository;   
            _transactionMapper = transactionMapper;
        }

        public async Task<IEnumerable<TransactionDto>> RetrieveTransactionsAsync(Guid userId)
        {
            var transactions = await _repository.GetAllAsync(userId);
            return _transactionMapper.MapElements(transactions.ToList());
        }
        public async Task<TransactionDto> RetrieveTransactionAsync(int id, Guid userId)
        {
            var transaction = await _repository.GetAsync(id, userId);
            if(transaction == null)
                throw new TransactionNotFoundException($"Transaction not found. Id:{id}.");
            return _transactionMapper.Map(transaction);
        }
        public async Task<TransactionDto> AddTransactionAsync(AddTransactionDto transaction)
        {
            if (transaction == null)
                throw new NullPointerException("Object is null");
            if (transaction.Name.Length < 3 || transaction.Name.Length > 30)
                throw new BadStringLengthException("Name have incorrect length. Should be more than 3 and less than 30.");
            if(!transaction.Description.IsNullOrEmpty())
            if (transaction.Description.Length < 3 || transaction.Description.Length > 150)
                throw new BadStringLengthException("Description have incorrect length. Should be more than 3 and less than 150.");
            if (transaction.Price < 0d)
                throw new BadValueException($"Price is incorrect. {transaction.Price}");
            
            Transaction mappedTransaction = _transactionMapper.Map(transaction);
            await _repository.AddAsync(mappedTransaction);

            return _transactionMapper.Map(mappedTransaction);

        }
        public async Task UpdateTransactionAsync(UpdateTransactionDto transaction)
        {
            if (transaction == null)
                throw new NullPointerException("Object is null");
            if (transaction.Name.Length < 3 || transaction.Name.Length > 30)
                throw new BadStringLengthException("Name have incorrect length. Should be more than 3 and less than 30.");
            if (!transaction.Description.IsNullOrEmpty())
                if (transaction.Description.Length < 3 || transaction.Description.Length > 150)
                    throw new BadStringLengthException("Description have incorrect length. Should be more than 3 and less than 150.");
            if (transaction.Price < 0d)
                throw new BadValueException($"Price is incorrect. {transaction.Price}");

            Transaction mappedTransaction = _transactionMapper.Map(transaction);
            await _repository.UpdateAsync(mappedTransaction);
        }

        public async Task DeleteTransactionAsync(int id, Guid userId)
        {
            var existingTransaction = await _repository.GetAsync(id, userId);
            if (existingTransaction == null)
                throw new NullPointerException($"Transaction not found. Id:{id}.");
            await _repository.DeleteAsync(id);
        }

        public async Task UpdateCategoryAsync(UpdateTransactionCategoryDto uc)
        {
            var existingTransaction = _repository.GetAsync(uc.Id, uc.UserId);
            if (existingTransaction == null)
                throw new TransactionNotFoundException($"Transaction not found. Id:{uc.Id}");
            if (!Enum.IsDefined(typeof(TransactionCategoryEnum), uc.Category))
                throw new BadValueException($"Category not found: {uc.Category}.");
            await _repository.UpdateCategoryAsync(uc);
        }
    }
}