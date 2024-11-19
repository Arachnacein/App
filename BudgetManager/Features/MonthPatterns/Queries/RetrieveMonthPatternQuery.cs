using BudgetManager.Data;
using BudgetManager.Dto.MonthPattern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.MonthPatterns.Queries
{
    public record RetrieveMonthPatternQuery : IRequest<MonthPatternDto>
    {
        public int Id { get; init; }
        public Guid UserId { get; set; }
        public RetrieveMonthPatternQuery(int id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
    public class RetrieveMonthPatternQueryhandler : IRequestHandler<RetrieveMonthPatternQuery, MonthPatternDto>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveMonthPatternQueryhandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MonthPatternDto> Handle(RetrieveMonthPatternQuery request, CancellationToken cancellationToken)
        {
            var query = await _dbContext.MonthPatterns
                                    .Where(x => x.Id == request.Id && x.UserId == request.UserId)
                                    .Select(mp => new MonthPatternDto
                                    {
                                        Id = mp.Id,
                                        UserId = mp.UserId,
                                        PatternId = mp.PatternId,
                                        Date = mp.Date
                                    })
                                    .FirstOrDefaultAsync(cancellationToken);
            return query;
        }
    }
}