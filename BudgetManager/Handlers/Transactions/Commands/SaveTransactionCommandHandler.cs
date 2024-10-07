using BudgetManager.Features.Transactions.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Handlers.Transactions.Commands
{
    public class SaveTransactionCommandHandler : IRequestHandler<SaveTransactionCommand>
    {
        private readonly DbContext _context;
        public Task Handle(SaveTransactionCommand request, CancellationToken cancellationToken)
        {
            return null; //temp ofc
        }
    }
}
