using BudgetManager.Data;
using BudgetManager.Dto.Pattern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Patterns.Queries
{
    public record RetrievePatternQuery : IRequest<PatternDto>
    {
        public int Id { get; init; }
        public RetrievePatternQuery(int id)
        {
            Id = id;
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
            var query = $"SELECT * FROM BudgetDB.dbo.Patterns WHERE Id = {request.Id}";
            var response = await _dbContext.Patterns
                                .FromSqlRaw(query)
                                .Select(pattern => new PatternDto
                                {
                                    Id = pattern.Id,
                                    Name = pattern.Name,
                                    Value_Saves = pattern.Value_Saves,
                                    Value_Fees = pattern.Value_Fees,
                                    Value_Entertainment = pattern.Value_Entertainment
                                })
                                .FirstOrDefaultAsync(cancellationToken);
            return response;
        }
    }
}