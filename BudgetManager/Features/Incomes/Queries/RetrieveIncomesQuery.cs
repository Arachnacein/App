﻿using BudgetManager.Data;
using BudgetManager.Dto.Income;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Incomes.Queries
{
    public record RetrieveIncomesQuery : IRequest<IEnumerable<IncomeDto>>
    {
        public Guid UserId { get; init; }
        public RetrieveIncomesQuery(Guid userId)
        {
            UserId = userId;
        }
    }
    public class RetrieveIncomesQueryHandler : IRequestHandler<RetrieveIncomesQuery, IEnumerable<IncomeDto>>
    {
        private readonly BudgetDbContext _dbContext;

        public RetrieveIncomesQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<IncomeDto>> Handle(RetrieveIncomesQuery request, CancellationToken cancellationToken)
        {
            var query = await _dbContext.Incomes
                            .Where(x => x.UserId == request.UserId)
                            .Select(income => new IncomeDto
                            {
                                Id = income.Id,
                                UserId = income.UserId,
                                Name = income.Name,
                                Amount = income.Amount,
                                Date = income.Date
                            })
                            .OrderByDescending(x => x.Date)
                            .ToListAsync(cancellationToken);
            return query;
        }
    }
}