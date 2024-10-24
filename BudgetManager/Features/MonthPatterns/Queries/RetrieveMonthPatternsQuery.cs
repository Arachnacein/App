﻿using BudgetManager.Data;
using BudgetManager.Dto.MonthPattern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.MonthPatterns.Queries
{
    public record RetrieveMonthPatternsQuery : IRequest<IEnumerable<MonthPatternDto>>
    {
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
            var query = "SELECT * FROM BudgetDB.dbo.MonthPatterns";
            var response = await _dbContext.MonthPatterns
                                .FromSqlRaw(query)
                                .Select(mp => new MonthPatternDto
                                {
                                    Id = mp.Id,
                                    PatternId = mp.PatternId,
                                    Date = mp.Date
                                })
                                .ToListAsync(cancellationToken);
            return response;
        }
    }
}