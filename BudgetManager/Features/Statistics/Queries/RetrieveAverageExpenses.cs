using BudgetManager.Data;
using BudgetManager.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveAverageExpenses : IRequest<double>
    {
    }
    public class RetrieveAverageExpensesHandler : IRequestHandler<RetrieveAverageExpenses, double>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveAverageExpensesHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<double> Handle(RetrieveAverageExpenses request, CancellationToken cancellationToken)
        {
            var averagExpenses = await _dbContext.Transactions
                                        .Where(x => x.Category != TransactionCategoryEnum.Saves)
                                        .AverageAsync(x => x.Price, cancellationToken);

            return Math.Round(averagExpenses,2);
        }
    }
}