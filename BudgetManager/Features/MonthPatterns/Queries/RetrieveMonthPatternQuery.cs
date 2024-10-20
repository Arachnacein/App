using BudgetManager.Data;
using BudgetManager.Dto.MonthPattern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.MonthPatterns.Queries
{
    public record RetrieveMonthPatternQuery : IRequest<MonthPatternDto>
    {
        public int Id { get; init; }
        public RetrieveMonthPatternQuery(int id)
        {
            Id = id;
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
            var query = $"SELECT * FROM BudgetDB.dbo.MonthPatterns WHERE Id = {request.Id}";
            var repsonse = await _dbContext.MonthPatterns
                                    .FromSqlRaw(query)
                                    .Select(mp => new MonthPatternDto
                                    {
                                        Id = mp.Id,
                                        PatternId = mp.PatternId,
                                        Date = mp.Date
                                    })
                                    .FirstOrDefaultAsync(cancellationToken);
            return repsonse;
        }
    }
}