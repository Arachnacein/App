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

            var query = 
              "SELECT p.Id, p.Name, p.Value_Saves, p.Value_Fees, p.Value_Entertainment" +
              " FROM [BudgetDB].[dbo].[Patterns] p" +
              " INNER JOIN [BudgetDB].[dbo].[MonthPatterns] mp ON p.Id = mp.PatternId "+
              $" WHERE MONTH(mp.Date) = {request.Month}"+
              $" AND YEAR(mp.Date) = {request.Year}";

            var response = await _dbContext.Patterns
                            .FromSqlRaw(query)
                            .FirstOrDefaultAsync(cancellationToken);

            if (response == null)
                throw new PatternNotFoundException($"Pattern not found. Id:{response.Id}.");

            return new PatternDto
            {
                Id = response.Id,
                Name = response.Name,
                Value_Saves = response.Value_Saves,
                Value_Fees = response.Value_Fees,
                Value_Entertainment = response.Value_Entertainment
            };

        }
    }
}