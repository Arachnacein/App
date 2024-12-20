﻿using BudgetManager.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Statistics.Queries
{
    public record RetrieveTotalExpensesQuery : IRequest<double>
    {
        public Guid UserId { get; init; }
        public RetrieveTotalExpensesQuery(Guid userId)
        {
            UserId = userId;
        }
    }
    public class RetrieveTotalExpensesQueryHandler : IRequestHandler<RetrieveTotalExpensesQuery, double>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveTotalExpensesQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<double> Handle(RetrieveTotalExpensesQuery request, CancellationToken cancellationToken)
        {
            var totalExpenses = await _dbContext.Transactions
                            .Where(x => x.Category != Models.TransactionCategoryEnum.Saves && 
                                        x.UserId == request.UserId)
                            .SumAsync(x => x.Price);
            return Math.Round(totalExpenses,2);
        }
    }
}