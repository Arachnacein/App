using BudgetManager.Data;
using BudgetManager.Dto.Pattern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Patterns.Queries
{
    public record RetrievePatternQuery : IRequest<PatternDto>
    {
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public RetrievePatternQuery(int id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
    public class RetrievePatternQueryHandler : IRequestHandler<RetrievePatternQuery, PatternDto>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrievePatternQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PatternDto> Handle(RetrievePatternQuery request, CancellationToken cancellationToken)
        {
            var query = await _dbContext.Patterns
                                .Where(x => x.Id == request.Id && x.UserId == request.UserId)
                                .Select(pattern => new PatternDto
                                {
                                    Id = pattern.Id,
                                    UserId = pattern.UserId,
                                    Name = pattern.Name,
                                    Value_Saves = pattern.Value_Saves,
                                    Value_Fees = pattern.Value_Fees,
                                    Value_Entertainment = pattern.Value_Entertainment
                                })
                                .FirstOrDefaultAsync(cancellationToken);
            return query;
        }
    }
}