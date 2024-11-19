using BudgetManager.Data;
using BudgetManager.Dto.Pattern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Patterns.Queries
{
    public record RetrievePatternsQuery : IRequest<IEnumerable<PatternDto>>
    {
        public Guid UserId { get; init; }
        public RetrievePatternsQuery(Guid userId)
        {
            UserId = userId;
        }
    }
    public class RetrievePatternsQueryHandler : IRequestHandler<RetrievePatternsQuery, IEnumerable<PatternDto>>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrievePatternsQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PatternDto>> Handle(RetrievePatternsQuery request, CancellationToken cancellationToken)
        {
            var query = await _dbContext.Patterns
                                    .Where(x => x.UserId == request.UserId)
                                    .Select(pattern => new PatternDto
                                    {
                                        Id = pattern.Id,
                                        UserId = pattern.UserId,
                                        Name = pattern.Name,
                                        Value_Saves = pattern.Value_Saves,
                                        Value_Entertainment = pattern.Value_Entertainment,
                                        Value_Fees = pattern.Value_Fees
                                    })
                                    .ToListAsync(cancellationToken);
            return query;
        }
    }
}