using BudgetManager.Data;
using BudgetManager.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveThreeMonthsSavesQuery : IRequest<double>
    {
    }
    public class RetrieveThreeMonthsSavesQueryHandler : IRequestHandler<RetrieveThreeMonthsSavesQuery, double>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveThreeMonthsSavesQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<double> Handle(RetrieveThreeMonthsSavesQuery request, CancellationToken cancellationToken)
        {
            var lastClosedMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
            var threeMonthsAgo = lastClosedMonth.AddMonths(-2);

            var saves = await _dbContext.Transactions
                                .Where(x => x.Category == TransactionCategoryEnum.Saves)
                                .Where(x => x.Date >= threeMonthsAgo && x.Date <= lastClosedMonth.AddMonths(1))
                                .SumAsync(x => x.Price, cancellationToken);
            return saves;
        }
    }
}