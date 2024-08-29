using BudgetManager.Dto;

namespace BudgetManager.Services
{
    public interface ITransactionService
    {
        IEnumerable<TransactionDto> RetrieveTransactions();
        TransactionDto RetrieveTransaction(int id);
        TransactionDto AddTransaction(AddTransactionDto transaction);
        void UpdateTransaction(UpdateTransactionDto transaction);
        void DeleteTransaction(int id);

    }
}
