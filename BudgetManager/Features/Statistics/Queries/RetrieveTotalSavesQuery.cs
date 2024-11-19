using BudgetManager.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveTotalSavesQuery : IRequest<double>
    {
        public Guid UserId { get; set; }
        public RetrieveTotalSavesQuery(Guid userId)
        {
            UserId = userId;
        }
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
                                    .Where(x => x.Category == Models.TransactionCategoryEnum.Saves && 
                                                x.UserId == request.UserId)
                                    .SumAsync(x => x.Price);
            return Math.Round(totalSaves,2);
        }
    }
}