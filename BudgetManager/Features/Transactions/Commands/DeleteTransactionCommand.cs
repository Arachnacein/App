using BudgetManager.Mappers;
using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.Transactions.Commands
{
    public class DeleteTransactionCommand : IRequest
    {
        public int Id { get; init; }
        public Guid UserId { get; set; }
        public DeleteTransactionCommand(int id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
    {
        private readonly ITransactionService _transactionService;

        public DeleteTransactionCommandHandler(ITransactionService transactionService, ITransactionMapper transactionMapper)
        {
            _transactionService = transactionService;
        }
        public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            await _transactionService.DeleteTransaction(request.Id, request.UserId);
        }
    }
}