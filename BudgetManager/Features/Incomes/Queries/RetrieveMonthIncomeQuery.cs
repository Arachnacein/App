using BudgetManager.Data;
using BudgetManager.Dto.Income;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Incomes.Queries
{
    public record RetrieveMonthIncomeQuery : IRequest<IEnumerable<IncomeDto>>
    {
        public int Month { get; init; }
        public int Year { get; init; }
        public RetrieveMonthIncomeQuery(int month, int year)
        {
            Month = month;
            Year = year;
        }
    }
    public class RetrieveMonthIncomeQueryHandler : IRequestHandler<RetrieveMonthIncomeQuery, IEnumerable<IncomeDto>>
    {
        private readonly BudgetDbContext _dbContext;
        public RetrieveMonthIncomeQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<IncomeDto>> Handle(RetrieveMonthIncomeQuery request, CancellationToken cancellationToken)
        {
            var query = $"SELECT * FROM BudgetDB.dbo.Incomes WHERE MONTH(Date) = {request.Month} AND YEAR(Date) = {request.Year}";
            var result = await _dbContext.Incomes
                                .FromSqlRaw(query)
                                .Select(income => new IncomeDto
                                {
                                    Id = income.Id,
                                    Name = income.Name,
                                    Amount = income.Amount,
                                    Date = income.Date
                                })
                                .ToListAsync(cancellationToken);
            return result;
        }
    }
}