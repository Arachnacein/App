using BudgetManager.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveCategoriesDistributionQuery : IRequest<CategoriesModel>
    {
    }
    public class RetrieveCategoriesDistributionQueryHandler : IRequestHandler<RetrieveCategoriesDistributionQuery, CategoriesModel>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveCategoriesDistributionQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CategoriesModel> Handle(RetrieveCategoriesDistributionQuery request, CancellationToken cancellationToken)
        {
            var totalTransactionAmount = await _dbContext.Transactions
                                .CountAsync(cancellationToken);

            var categoryCount = await _dbContext.Transactions
                                .GroupBy(x => x.Category)
                                .Select(x => new
                                {
                                    Category = x.Key,
                                    Count = x.Count()
                                })
                                .ToListAsync(cancellationToken);
            var categoriesModel = new CategoriesModel
            {
                Saves = (categoryCount.FirstOrDefault(x => x.Category == Models.TransactionCategoryEnum.Saves)?.Count ?? 0) / (double)totalTransactionAmount * 100,
                Fees = (categoryCount.FirstOrDefault(x => x.Category == Models.TransactionCategoryEnum.Fees)?.Count ?? 0) / (double)totalTransactionAmount * 100,
                Enterntainment = (categoryCount.FirstOrDefault(x => x.Category == Models.TransactionCategoryEnum.Entertainment)?.Count ?? 0) / (double)totalTransactionAmount * 100,
            };

            return categoriesModel;
        }
    }
    public class CategoriesModel
    {
        public double Saves { get; set; }
        public double Fees { get; set; }
        public double Enterntainment { get; set; }
    }
}