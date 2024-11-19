using BudgetManager.Data;
using BudgetManager.Dto.MonthPattern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.MonthPatterns.Queries
{
    public record RetrieveMonthPatternsQuery : IRequest<IEnumerable<MonthPatternDto>>
    {
        public Guid UserId { get; init; }
        public RetrieveMonthPatternsQuery(Guid userId)
        {
            UserId = userId;
        }
    }
    public class RetrieveMonthPatternsQueryHandler : IRequestHandler<RetrieveMonthPatternsQuery, IEnumerable<MonthPatternDto>>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveMonthPatternsQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<MonthPatternDto>> Handle(RetrieveMonthPatternsQuery request, CancellationToken cancellationToken)
        {
            var query = await _dbContext.MonthPatterns
                                .Where(x => x.UserId == request.UserId)
                                .Select(mp => new MonthPatternDto
                                {
                                    Id = mp.Id,
                                    UserId = mp.UserId,
                                    PatternId = mp.PatternId,
                                    Date = mp.Date
                                })
                                .ToListAsync(cancellationToken);
            return query;
        }
    }
}