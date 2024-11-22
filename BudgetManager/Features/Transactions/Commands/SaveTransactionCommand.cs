using BudgetManager.Dto.Transaction;
using BudgetManager.Mappers;
using BudgetManager.Models;
using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.Transactions.Commands
{
    public record SaveTransactionCommand : IRequest<TransactionDto>
    {
        public string Name { get; init; }
        public Guid UserId { get; init; }
        public string? Description { get; init; }
        public DateTime Date { get; init; }
        public double Price { get; init; }
        public TransactionCategoryEnum Category { get; set; }

        public SaveTransactionCommand(string name, string? description, DateTime date, double price, TransactionCategoryEnum category, Guid userId)
        {
            Name = name;
            Description = description;
            Date = date;
            Price = price;
            Category = category;
            UserId = userId;
        }
    }
    public class SaveTransactionCommandHandler : IRequestHandler<SaveTransactionCommand, TransactionDto>
    {
        private readonly ITransactionService _transactionService;
        private readonly ITransactionMapper _transactionMapper;

        public SaveTransactionCommandHandler(ITransactionService transactionService, ITransactionMapper transactionMapper)
        {
            _transactionService = transactionService;
            _transactionMapper = transactionMapper;
        }
        public async Task<TransactionDto> Handle(SaveTransactionCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _transactionService.AddTransactionAsync(_transactionMapper.Map(request));
        }
    }
}