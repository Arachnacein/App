using BudgetManager.Dto.Transaction;
using BudgetManager.Models;
using MediatR;

namespace BudgetManager.Features.Transactions.Commands
{
    public class SaveTransactionCommand : IRequest<TransactionDto>
    {
        public string Name { get; init; }
        public string? Description { get; init; }
        public DateTime Date { get; init; }
        public double Price { get; init; }
        public TransactionCategoryEnum Category { get; set; }

        public SaveTransactionCommand(string name, string? description, DateTime date, double price, TransactionCategoryEnum category)
        {
            Name = name;
            Description = description;
            Date = date;
            Price = price;
            Category = category;
        }
    }
}