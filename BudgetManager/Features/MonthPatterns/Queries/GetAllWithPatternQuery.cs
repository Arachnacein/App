using BudgetManager.Data;
using BudgetManager.Dto.MonthPattern;
using BudgetManager.Dto.Pattern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.MonthPatterns.Queries
{
    public record GetAllWithPatternQuery : IRequest<IEnumerable<FullMonthPatternDto>>
    {
    }
    public class GetAllWithPatternQueryHandler : IRequestHandler<GetAllWithPatternQuery, IEnumerable<FullMonthPatternDto>>
    {
        private readonly BudgetDbContext _dbContext;
        public GetAllWithPatternQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FullMonthPatternDto>> Handle(GetAllWithPatternQuery request, CancellationToken cancellationToken)
        {
            var query =
                "SELECT" +
                " mp.Id AS MonthPatternID, " +
                " mp.Date, " +
                " mp.PatternId AS MonthPatterPatternID," +
                " p.Id AS PatternID, " +
                " p.Name, " +
                " p.Value_Saves, " +
                " p.Value_Fees, " +
                " p.Value_Entertainment" +
                " FROM BudgetDB.dbo.MonthPatterns mp" +
                " INNER JOIN BudgetDB.dbo.Patterns p" +
                " ON mp.PatternId = p.Id";

            var response = await _dbContext.MonthPatterns
                            .FromSqlRaw(query)
                            .Select(mp => new FullMonthPatternDto
                            {
                                Id = mp.Id,
                                Date = mp.Date,
                                Pattern = new PatternDto
                                {
                                    Id = mp.Pattern.Id,
                                    Name = mp.Pattern.Name,
                                    Value_Saves = mp.Pattern.Value_Saves,
                                    Value_Fees = mp.Pattern.Value_Fees,
                                    Value_Entertainment = mp.Pattern.Value_Entertainment
                                }
                            })
                            .OrderByDescending(x => x.Date)
                            .ToListAsync(cancellationToken);
            return response;
        }
    }
}