using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.RecurringTransactions.Commands
{
    public record DeleteRecurringTransactionCommand : IRequest
    {
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public DeleteRecurringTransactionCommand(int id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
    public class DeleteRecurringTransactionCommandHandler : IRequestHandler<DeleteRecurringTransactionCommand>
    {
        private readonly IRecurringTransactionService _recurringTransactionService;
        public DeleteRecurringTransactionCommandHandler(IRecurringTransactionService recurringTransactionService)
        {
            _recurringTransactionService = recurringTransactionService;
        }
        public async Task Handle(DeleteRecurringTransactionCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));
            await _recurringTransactionService.DeleteRecurringTransactionAsync(request.Id, request.UserId);
        }
    }
}