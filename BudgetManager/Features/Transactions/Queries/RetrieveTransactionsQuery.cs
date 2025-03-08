using BudgetManager.Data;
using BudgetManager.Dto.Transaction;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Transactions.Queries
{
    public record RetrieveTransactionsQuery : IRequest<IEnumerable<TransactionDto>>
    {
        public Guid UserId { get; init; }
        public RetrieveTransactionsQuery(Guid userId)
        {
            UserId = userId;
        }
    }

    public class RetrieveTransactionsQueryhandler : IRequestHandler<RetrieveTransactionsQuery, IEnumerable<TransactionDto>>
    {
        private readonly BudgetDbContext _dbContext;

        public RetrieveTransactionsQueryhandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TransactionDto>> Handle(RetrieveTransactionsQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _dbContext.Transactions
                                .Where(x => x.UserId == request.UserId)
                                .Select(transaction => new TransactionDto
                                {
                                    Id = transaction.Id,
                                    UserId = transaction.UserId,
                                    Name = transaction.Name,
                                    Description = transaction.Description,
                                    Date = transaction.Date,
                                    Price = transaction.Price,
                                    Category = transaction.Category,
                                    IsRecurring = transaction.IsRecurring,
                                    IsApproved = transaction.IsApproved
                                })
                                .OrderByDescending(x => x.Date)
                                .ToListAsync(cancellationToken);
            return transactions;
        }
    }
}