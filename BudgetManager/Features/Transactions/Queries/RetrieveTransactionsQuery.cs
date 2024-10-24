﻿using BudgetManager.Data;
using BudgetManager.Dto.Transaction;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Transactions.Queries
{
    public record RetrieveTransactionsQuery : IRequest<IEnumerable<TransactionDto>>
    { }

    public class RetrieveTransactionsQueryhandler : IRequestHandler<RetrieveTransactionsQuery, IEnumerable<TransactionDto>>
    {
        private readonly BudgetDbContext _dbContext;

        public RetrieveTransactionsQueryhandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TransactionDto>> Handle(RetrieveTransactionsQuery request, CancellationToken cancellationToken)
        {
            var query = "SELECT * FROM BudgetDB.dbo.Transactions";
            var response = await _dbContext.Transactions
                                .FromSqlRaw(query)
                                .Select(transaction => new TransactionDto
                                {
                                    Id = transaction.Id,
                                    Name = transaction.Name,
                                    Description = transaction.Description,
                                    Date = transaction.Date,
                                    Price = transaction.Price,
                                    Category = transaction.Category
                                })
                                .ToListAsync(cancellationToken);
            return response;
        }
    }
}