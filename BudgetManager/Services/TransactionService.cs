using BudgetManager.Dto;
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

        public IEnumerable<TransactionDto> RetrieveTransactions()
        {
            var transactions = _repository.GetAll();
            return _transactionMapper.MapElements(transactions.ToList());
        }
        public TransactionDto RetrieveTransaction(int id)
        {
            var transaction = _repository.Get(id);
            if(transaction == null)
                throw new TransactionNotFoundException($"Transaction not found. Id:{id}.");
            return _transactionMapper.Map(transaction);
        }
        public TransactionDto AddTransaction(AddTransactionDto transaction)
        {
            if (transaction == null)
                throw new NullPointerException("Object is null");
            if (transaction.Name.Length < 5 || transaction.Name.Length > 15)
                throw new BadStringLengthException("Name have incorrect length. Should be more than 5 and less than 15.");
            if(!transaction.Description.IsNullOrEmpty())
            if (transaction.Description.Length < 5 || transaction.Description.Length > 50)
                throw new BadStringLengthException("Description have incorrect length. Should be more than 5 and less than 50.");
            if (transaction.Price < 0d)
                throw new BadValueException($"Price is incorrect. {transaction.Price}");
            
            Transaction mappedTransaction = _transactionMapper.Map(transaction);
            _repository.Add(mappedTransaction);

            return _transactionMapper.Map(mappedTransaction);

        }
        public void UpdateTransaction(UpdateTransactionDto transaction)
        {
            if (transaction == null)
                throw new NullPointerException("Object is null");
            if (transaction.Name.Length < 5 || transaction.Name.Length > 15)
                throw new BadStringLengthException("Name have incorrect length. Should be more than 5 and less than 15.");
            if (!transaction.Description.IsNullOrEmpty())
                if (transaction.Description.Length < 5 || transaction.Description.Length > 50)
                    throw new BadStringLengthException("Description have incorrect length. Should be more than 5 and less than 50.");
            if (transaction.Price < 0d)
                throw new BadValueException($"Price is incorrect. {transaction.Price}");

            Transaction mappedTransaction = _transactionMapper.Map(transaction);
            _repository.Update(mappedTransaction);
        }

        public void DeleteTransaction(int id)
        {
            var existingTransaction = _repository.Get(id);
            if (existingTransaction == null)
                throw new TransactionNotFoundException($"Transaction not found. Id:{id}.");
            _repository.Delete(id);
        }
    }
}
