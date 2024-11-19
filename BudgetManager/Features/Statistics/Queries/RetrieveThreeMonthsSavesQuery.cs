using BudgetManager.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveThreeMonthsSavesQuery : IRequest<double>
    {
        public Guid UserId { get; init; }
        public RetrieveThreeMonthsSavesQuery(Guid userId)
        {
            UserId = userId;
        }
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
                                .Where(x => x.Category == Models.TransactionCategoryEnum.Saves &&
                                            x.UserId == request.UserId)
                                .Where(x => x.Date >= threeMonthsAgo && x.Date <= lastClosedMonth.AddMonths(1))
                                .SumAsync(x => x.Price, cancellationToken);
            return Math.Round(saves, 2);
        }
    }
}