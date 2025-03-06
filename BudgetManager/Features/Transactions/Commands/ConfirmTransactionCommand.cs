using BudgetManager.Mappers;
using BudgetManager.Repositories;
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
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionMapper _mapper;
        public ConfirmTransactionCommandHandler(ITransactionRepository transactionRepository, ITransactionMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task Handle(ConfirmTransactionCommand request, CancellationToken cancellationToken)
        {
            var mappedTransaction = _mapper.Map(request);
            await _transactionRepository.ConfirmTransactionAsync(mappedTransaction);
        }
    }
}
