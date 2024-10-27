using BudgetManager.Data;
using BudgetManager.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveTotalExpensesQuery : IRequest<double>
    {
    }
    public class RetrieveTotalExpensesQueryHandler : IRequestHandler<RetrieveTotalExpensesQuery, double>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveTotalExpensesQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<double> Handle(RetrieveTotalExpensesQuery request, CancellationToken cancellationToken)
        {
            var totalExpenses = await _dbContext.Transactions
                            .Where(x => x.Category == TransactionCategoryEnum.Fees ||
                                        x.Category == TransactionCategoryEnum.Entertainment)
                            .SumAsync(x => x.Price);
            return totalExpenses;
        }
    }
}