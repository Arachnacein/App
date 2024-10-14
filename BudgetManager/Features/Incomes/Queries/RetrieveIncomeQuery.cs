using BudgetManager.Data;
using BudgetManager.Dto.Income;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Incomes.Queries
{
    public record RetrieveIncomeQuery : IRequest<IncomeDto>
    {
        public int Id { get; set; }
        public RetrieveIncomeQuery(int id)
        {
            Id = id;
        }
    }
    public class RetrieveIncomeQueryHandler : IRequestHandler<RetrieveIncomeQuery, IncomeDto>
    {
        private readonly BudgetDbContext _dbContext;

        public RetrieveIncomeQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IncomeDto> Handle(RetrieveIncomeQuery request, CancellationToken cancellationToken)
        {
            var query = $"SELECT * FROM BudgetDB.dbo.Incomes WHERE Id = {request.Id}";
            var result = _dbContext.Incomes
                            .FromSqlRaw(query)
                            .Select(income => new IncomeDto
                            {
                                Id = income.Id,
                                Name = income.Name,
                                Amount = income.Amount,
                                Date = income.Date
                            })
                            .FirstOrDefaultAsync(cancellationToken);
            return result;
        }
    }
}