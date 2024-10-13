using BudgetManager.Data;
using BudgetManager.Dto.Transaction;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Transactions.Queries
{
    public record RetrieveTransactionQuery : IRequest<TransactionDto>
    {
        public int Id { get; set; }
        public RetrieveTransactionQuery(int id)
        {
            Id = id;
        }
    }
    public class RetrieveTransactionQueryHandler : IRequestHandler<RetrieveTransactionQuery, TransactionDto>
    {
        private readonly BudgetDbContext _dbContext;

        public RetrieveTransactionQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TransactionDto> Handle(RetrieveTransactionQuery request, CancellationToken cancellationToken)
        {
            var query = $"SELECT * FROM BudgetDB.dbo.Transactions WHERE Id = {request.Id}";
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
                                    .FirstOrDefaultAsync(cancellationToken);

            if(response == null)
                throw new ArgumentNullException(nameof(response));

            return response;
        }
    }
}