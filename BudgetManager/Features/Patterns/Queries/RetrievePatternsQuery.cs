using BudgetManager.Data;
using BudgetManager.Dto.Pattern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Patterns.Queries
{
    public record RetrievePatternsQuery : IRequest<IEnumerable<PatternDto>>
    {
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
            var query = "SELECT * FROM BudgetDB.dbo.Patterns";
            var response = await _dbContext.Patterns
                                    .FromSqlRaw(query)
                                    .Select(pattern => new PatternDto
                                    {
                                        Id = pattern.Id,
                                        Name = pattern.Name,
                                        Value_Saves = pattern.Value_Saves,
                                        Value_Entertainment = pattern.Value_Entertainment,
                                        Value_Fees = pattern.Value_Fees
                                    })
                                    .ToListAsync(cancellationToken);
            return response;
        }
    }
}