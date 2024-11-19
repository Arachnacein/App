using BudgetManager.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveAverageSavesQuery : IRequest<double>
    {
        public Guid UserId { get; init; }
        public RetrieveAverageSavesQuery(Guid userId)
        {
            UserId = userId;
        }
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
            var saves = await _dbContext.Transactions
                                        .Where(x => x.Category == Models.TransactionCategoryEnum.Saves &&
                                               x.UserId == request.UserId)
                                        .ToListAsync(cancellationToken);

            if (!saves.Any())
                return 0;
            else return Math.Round(saves.Average(x => x.Price), 2);
        }
    }
}