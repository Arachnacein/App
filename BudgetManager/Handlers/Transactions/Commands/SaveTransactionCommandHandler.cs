using BudgetManager.Dto.Transaction;
using BudgetManager.Features.Transactions.Commands;
using BudgetManager.Mappers;
using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Handlers.Transactions.Commands
{
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
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            return await _transactionService.AddTransaction(_transactionMapper.Map(request));
        }
    }
}