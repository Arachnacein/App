using BudgetManager.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveAverageExpensesQuery : IRequest<double>
    {
        public Guid UserId { get; init; }
        public RetrieveAverageExpensesQuery(Guid userId)
        {
            UserId = userId;
        }
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
            var expenses = await _dbContext.Transactions
                                        .Where(x => x.Category != Models.TransactionCategoryEnum.Saves && 
                                                    x.UserId == request.UserId)
                                        .ToListAsync(cancellationToken);
            
            if (!expenses.Any())
                return 0;                    
            else return Math.Round(expenses.Average(x => x.Price),2);
        }
    }
}