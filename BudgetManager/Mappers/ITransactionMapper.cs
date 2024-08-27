using BudgetManager.Dto;
using BudgetManager.Models;

namespace BudgetManager.Mappers
{
    public interface ITransactionMapper
    {
        TransactionDto Map(Transaction source);
        Transaction Map(TransactionDto source);
        Transaction Map(AddTransactionDto source);
        Transaction Map(UpdateTransactionDto source);
        ICollection<TransactionDto> MapElements(ICollection<Transaction> source);
        ICollection<Transaction> MapElements(ICollection<TransactionDto> source);
    }
}
