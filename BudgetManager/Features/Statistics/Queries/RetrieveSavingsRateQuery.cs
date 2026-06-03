namespace BudgetManager.Features.Statistics.Queries;

public record RetrieveSavingsRateQuery : IRequest<double>
{
    public Guid UserId { get; init; }
    public RetrieveSavingsRateQuery(Guid userId)
    {
        UserId = userId;
    }
}
public class RetrieveSavingsRateQueryHandler : IRequestHandler<RetrieveSavingsRateQuery, double>
{
    private readonly BudgetDbContext _dbContext;
    public RetrieveSavingsRateQueryHandler(BudgetDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<double> Handle(RetrieveSavingsRateQuery request, CancellationToken cancellationToken)
    {
        var total = await _dbContext.Transactions
            .Where(x => x.UserId == request.UserId)
            .SumAsync(x => x.Price, cancellationToken);

        if (total == 0)
            return 0;

        var totalSaves = await _dbContext.Transactions
            .Where(x => x.UserId == request.UserId && x.Category == TransactionCategoryEnum.Saves)
            .SumAsync(x => x.Price, cancellationToken);

        return Math.Round(totalSaves / total * 100, 1);
    }
}
