using BudgetManager.Dto.Transaction;
using BudgetManager.Models;

namespace BudgetManager.Mappers
{
    public class TransactionMapper : ITransactionMapper
    {
        public TransactionDto Map(Transaction source)
        {
            var destination = new TransactionDto();
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Date = source.Date;
            destination.Price = source.Price;
            destination.Category = source.Category;

            return destination;
        }

        public Transaction Map(TransactionDto source)
        {
            var destination = new Transaction();
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Date = source.Date;
            destination.Price = source.Price;
            destination.Category = source.Category;

            return destination;
        }

        public Transaction Map(AddTransactionDto source)
        {
            var destination = new Transaction();
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Date = source.Date;
            destination.Price = source.Price;
            destination.Category = source.Category;

            return destination;
        }

        public Transaction Map(UpdateTransactionDto source)
        {
            var destination = new Transaction();
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Date = source.Date;
            destination.Price = source.Price;
            destination.Category = source.Category;

            return destination;
        }
        public ICollection<TransactionDto> MapElements(ICollection<Transaction> source)
        {
            List<TransactionDto> destination = new List<TransactionDto>();

            foreach (var transaction in source)
                destination.Add(Map(transaction));

            return destination;
        }

        public ICollection<Transaction> MapElements(ICollection<TransactionDto> source)
        {
            List<Transaction> destination = new List<Transaction>();

            foreach (var transaction in source)
                destination.Add(Map(transaction));

            return destination;
        }
    }
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
