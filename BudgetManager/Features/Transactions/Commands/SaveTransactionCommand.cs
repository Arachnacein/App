using MediatR;

namespace BudgetManager.Features.Transactions.Commands
{
    public class SaveTransactionCommand : IRequest
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public DateTime Date { get; init; }
        public double Price { get; init; }
    }
}
