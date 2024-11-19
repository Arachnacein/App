using BudgetManager.Mappers;
using BudgetManager.Models;
using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.Transactions.Commands
{
    public record UpdateCategoryCommand : IRequest
    {
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public TransactionCategoryEnum Category { get; set; }

        public UpdateCategoryCommand(int id, Guid userId, TransactionCategoryEnum category)
        {
            Id = id;
            Category = category;
            UserId = userId;
        }
    }
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly ITransactionService _transactionService;
        private readonly ITransactionMapper _transactionMapper;

        public UpdateCategoryCommandHandler(ITransactionService transactionService, ITransactionMapper transactionMapper)
        {
            _transactionService = transactionService;
            _transactionMapper = transactionMapper;
        }
        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            await _transactionService.UpdateCategory(_transactionMapper.Map(request));
        }
    }
}
