namespace BudgetManager.Features.Statistics.Queries;

public record RetrieveAvailableYearsQuery : IRequest<List<int>>
{
    public Guid UserId { get; init; }
    public RetrieveAvailableYearsQuery(Guid userId)
    {
        UserId = userId;
    }
}
public class RetrieveAvailableYearsQueryHandler : IRequestHandler<RetrieveAvailableYearsQuery, List<int>>
{
    private readonly BudgetDbContext _dbContext;
    public RetrieveAvailableYearsQueryHandler(BudgetDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<int>> Handle(RetrieveAvailableYearsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Transactions
            .Where(x => x.UserId == request.UserId)
            .Select(x => x.Date.Year)
            .Distinct()
            .OrderByDescending(y => y)
            .ToListAsync(cancellationToken);
    }
}
