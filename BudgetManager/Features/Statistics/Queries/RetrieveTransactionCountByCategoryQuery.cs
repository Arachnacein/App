using BudgetManager.Data;
using BudgetManager.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveTransactionCountByCategoryQuery : IRequest<TransactionCountModel>
    {
        public Guid UserId { get; init; }
        public RetrieveTransactionCountByCategoryQuery(Guid userId)
        {
            UserId = userId;
        }
    }
    public class RetrieveTransactionCountByCategoryQueryHandler : IRequestHandler<RetrieveTransactionCountByCategoryQuery, TransactionCountModel>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveTransactionCountByCategoryQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TransactionCountModel> Handle(RetrieveTransactionCountByCategoryQuery request, CancellationToken cancellationToken)
        {
            var counts = await _dbContext.Transactions
                .Where(x => x.UserId == request.UserId)
                .GroupBy(x => x.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            return new TransactionCountModel
            {
                Saves = counts.FirstOrDefault(x => x.Category == TransactionCategoryEnum.Saves)?.Count ?? 0,
                Fees = counts.FirstOrDefault(x => x.Category == TransactionCategoryEnum.Fees)?.Count ?? 0,
                Entertainment = counts.FirstOrDefault(x => x.Category == TransactionCategoryEnum.Entertainment)?.Count ?? 0
            };
        }
    }
}
