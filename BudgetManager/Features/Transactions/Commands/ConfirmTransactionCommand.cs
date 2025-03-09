using BudgetManager.Mappers;
using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.Transactions.Commands
{
    public record ConfirmTransactionCommand : IRequest
    {
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public ConfirmTransactionCommand(int id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
    public class ConfirmTransactionCommandHandler : IRequestHandler<ConfirmTransactionCommand>
    {
        private readonly ITransactionService _transactionService;
        private readonly ITransactionMapper _mapper;
        public ConfirmTransactionCommandHandler(ITransactionService transactionService, ITransactionMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        public async Task Handle(ConfirmTransactionCommand request, CancellationToken cancellationToken)
        {
            var mappedTransaction = _mapper.Map(request);
            await _transactionService.ConfirmTransactionAsync(mappedTransaction);
        }
    }
}
