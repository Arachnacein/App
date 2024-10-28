using BudgetManager.Data;
using BudgetManager.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveAverageExpensesQuery : IRequest<double>
    {
    }
    public class RetrieveAverageExpensesQueryHandler : IRequestHandler<RetrieveAverageExpensesQuery, double>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveAverageExpensesQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<double> Handle(RetrieveAverageExpensesQuery request, CancellationToken cancellationToken)
        {
            var averagExpenses = await _dbContext.Transactions
                                        .Where(x => x.Category != TransactionCategoryEnum.Saves)
                                        .AverageAsync(x => x.Price, cancellationToken);

            return Math.Round(averagExpenses,2);
        }
    }
}