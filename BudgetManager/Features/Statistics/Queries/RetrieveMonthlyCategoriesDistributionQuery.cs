using BudgetManager.Data;
using BudgetManager.Models;
using MediatR;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveMonthlyCategoriesDistributionQuery : IRequest<List<MonthlyCategoriesModel>>
    {
    }
    public class RetrieveMonthlyCategoriesDistributionQueryHandler : IRequestHandler<RetrieveMonthlyCategoriesDistributionQuery, List<MonthlyCategoriesModel>>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveMonthlyCategoriesDistributionQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MonthlyCategoriesModel>> Handle(RetrieveMonthlyCategoriesDistributionQuery request, CancellationToken cancellationToken)
        {
                var minTransactionDate = _dbContext.Transactions.Min(x => x.Date);
                var maxTransactionDate = _dbContext.Transactions.Max(x => x.Date);
                var monthlyCategories = new List<MonthlyCategoriesModel>();
            
                for (var date = new DateTime(minTransactionDate.Year, minTransactionDate.Month, 1);
                         date <= maxTransactionDate;
                         date = date.AddMonths(1))
                {
                    var transactionsInMonth = _dbContext.Transactions
                                      .Where(t => t.Date.Year == date.Year && t.Date.Month == date.Month)
                                      .ToList();

                    int totalTransactionsInMonthAmount = transactionsInMonth.Count;
                    if (totalTransactionsInMonthAmount == 0) 
                        continue;

                    monthlyCategories.Add(new MonthlyCategoriesModel
                    {
                        Month = date.Month,
                        Year = date.Year,
                        Saves = (double)transactionsInMonth.Count(x => x.Category == TransactionCategoryEnum.Saves) / totalTransactionsInMonthAmount * 100,
                        Fees = (double)transactionsInMonth.Count(x => x.Category == TransactionCategoryEnum.Fees) / totalTransactionsInMonthAmount * 100,
                        Entertainment = (double)transactionsInMonth.Count(x => x.Category == TransactionCategoryEnum.Entertainment) / totalTransactionsInMonthAmount * 100
                    });

                }

                return monthlyCategories;
        }
    }
}