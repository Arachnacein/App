using BudgetManager.Data;
using BudgetManager.Dto.Income;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.Incomes.Queries
{
    public record RetrieveIncomeQuery : IRequest<IncomeDto>
    {
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public RetrieveIncomeQuery(int id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
    public class RetrieveIncomeQueryHandler : IRequestHandler<RetrieveIncomeQuery, IncomeDto>
    {
        private readonly BudgetDbContext _dbContext;

        public RetrieveIncomeQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IncomeDto> Handle(RetrieveIncomeQuery request, CancellationToken cancellationToken)
        {
            var query = await _dbContext.Incomes
                            .Where(x => x.Id == request.Id && x.UserId == request.UserId)
                            .Select(income => new IncomeDto
                            {
                                Id = income.Id,
                                UserId = income.UserId,
                                Name = income.Name,
                                Amount = income.Amount,
                                Date = income.Date
                            })
                            .FirstOrDefaultAsync(cancellationToken);

            return query;
        }
    }
}