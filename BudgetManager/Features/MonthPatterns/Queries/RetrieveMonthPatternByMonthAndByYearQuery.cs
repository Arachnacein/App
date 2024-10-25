using BudgetManager.Data;
using BudgetManager.Dto.MonthPattern;
using BudgetManager.Dto.Pattern;
using BudgetManager.Exceptions.PatternExceptions;
using BudgetManager.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.MonthPatterns.Queries
{
    public record RetrieveMonthPatternByMonthAndByYearQuery : IRequest<PatternDto>
    {
        public int Month { get; init; }
        public int Year { get; init; }
        public RetrieveMonthPatternByMonthAndByYearQuery(int month, int year)
        {
            Month = month;
            Year = year;
        }
    }
    public class RetrieveMonthPatternByMonthAndByYearQueryHandler : IRequestHandler<RetrieveMonthPatternByMonthAndByYearQuery, PatternDto>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveMonthPatternByMonthAndByYearQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PatternDto> Handle(RetrieveMonthPatternByMonthAndByYearQuery request, CancellationToken cancellationToken)
        {
            var monthPatternExists = await _dbContext.MonthPatterns
                    .AnyAsync(mp => mp.Date.Month == request.Month && 
                              mp.Date.Year == request.Year, 
                              cancellationToken);

            if (!monthPatternExists)
                return new PatternDto { Id = -1 };

            var pattern = await _dbContext.MonthPatterns
                                .Where(x => x.Date.Month == request.Month &&
                                            x.Date.Year == request.Year)
                                .Select(x => x.Pattern)
                                .FirstOrDefaultAsync(cancellationToken);


            if (pattern == null)
                throw new PatternNotFoundException($"Pattern not found. Id:{pattern.Id}.");

            return new PatternDto
            {
                Id = pattern.Id,
                Name = pattern.Name,
                Value_Saves = pattern.Value_Saves,
                Value_Fees = pattern.Value_Fees,
                Value_Entertainment = pattern.Value_Entertainment
            };

        }
    }
}