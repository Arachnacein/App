using BudgetManager.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveTotalSavesQuery : IRequest<double>
    {
    }
    public class RetrieveTotalSavesQueryHandler : IRequestHandler<RetrieveTotalSavesQuery, double>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveTotalSavesQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<double> Handle(RetrieveTotalSavesQuery request, CancellationToken cancellationToken)
        {
            var totalSaves = await _dbContext.Transactions
                                    .Where(x => x.Category == Models.TransactionCategoryEnum.Saves)
                                    .SumAsync(x => x.Price);
            return Math.Round(totalSaves,2);
        }
    }
}