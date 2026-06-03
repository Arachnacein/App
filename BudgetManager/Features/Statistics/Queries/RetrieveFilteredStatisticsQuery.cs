using BudgetManager.Data;
using BudgetManager.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries;

public record RetrieveFilteredStatisticsQuery : IRequest<FilteredStatisticsModel>
{
    public Guid UserId { get; init; }
    public int? Year { get; init; }
    public RetrieveFilteredStatisticsQuery(Guid userId, int? year)
    {
        UserId = userId;
        Year = year;
    }
}
public class RetrieveFilteredStatisticsQueryHandler : IRequestHandler<RetrieveFilteredStatisticsQuery, FilteredStatisticsModel>
{
    private readonly BudgetDbContext _dbContext;
    public RetrieveFilteredStatisticsQueryHandler(BudgetDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<FilteredStatisticsModel> Handle(RetrieveFilteredStatisticsQuery request, CancellationToken cancellationToken)
    {
        var txs = await _dbContext.Transactions
            .Where(x => x.UserId == request.UserId &&
                        (!request.Year.HasValue || x.Date.Year == request.Year.Value))
            .ToListAsync(cancellationToken);

        if (!txs.Any())
            return new FilteredStatisticsModel { TransactionCount = new TransactionCountModel() };

        var saves   = txs.Where(x => x.Category == TransactionCategoryEnum.Saves).ToList();
        var expenses = txs.Where(x => x.Category != TransactionCategoryEnum.Saves).ToList();
        var totalPrice = txs.Sum(x => x.Price);

        return new FilteredStatisticsModel
        {
            TotalSaves      = Math.Round(saves.Sum(x => x.Price), 2),
            TotalExpenses   = Math.Round(expenses.Sum(x => x.Price), 2),
            AverageSaves    = saves.Any()    ? Math.Round(saves.Average(x => x.Price), 2)    : 0,
            AverageExpenses = expenses.Any() ? Math.Round(expenses.Average(x => x.Price), 2) : 0,
            SavingsRate     = totalPrice == 0 ? 0 : Math.Round(saves.Sum(x => x.Price) / totalPrice * 100, 1),
            TransactionCount = new TransactionCountModel
            {
                Saves         = saves.Count,
                Fees          = txs.Count(x => x.Category == TransactionCategoryEnum.Fees),
                Entertainment = txs.Count(x => x.Category == TransactionCategoryEnum.Entertainment)
            }
        };
    }
}
