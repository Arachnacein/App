using BudgetManager.Data;
using BudgetManager.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveThreeMonthsExpensesQuery : IRequest<double>
    {
    }
    public class RetrieveThreeMonthsExpensesQueryHandler : IRequestHandler<RetrieveThreeMonthsExpensesQuery, double>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveThreeMonthsExpensesQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<double> Handle(RetrieveThreeMonthsExpensesQuery request, CancellationToken cancellationToken)
        {
            var lastClosedMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
            var threeMonthsAgo = lastClosedMonth.AddMonths(-2);

            var expenses = await _dbContext.Transactions
                                .Where(x => x.Category != TransactionCategoryEnum.Saves)
                                .Where(x => x.Date >= threeMonthsAgo && x.Date <= lastClosedMonth.AddMonths(1))
                                .SumAsync(x => x.Price, cancellationToken);
            return expenses;
        }
    }
}