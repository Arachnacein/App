using BudgetManager.Data;
using BudgetManager.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveAverageSavesQuery : IRequest<double>
    {
    }
    public class RetrieveAverageSavesQueryHandler : IRequestHandler<RetrieveAverageSavesQuery, double>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveAverageSavesQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<double> Handle(RetrieveAverageSavesQuery request, CancellationToken cancellationToken)
        {
            var averageSaves = await _dbContext.Transactions
                                        .Where(x => x.Category == Models.TransactionCategoryEnum.Saves)
                                        .AverageAsync(x => x.Price, cancellationToken);
            return Math.Round(averageSaves, 2);
        }
    }
}