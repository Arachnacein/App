using BudgetManager.Data;
using BudgetManager.Dto.RecurringTransaction;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.RecurringTransactions.Queries
{
    public record RetrieveRecurringTransactionsQuery : IRequest<IEnumerable<RecurringTransactionDto>>
    {
        public Guid UserId { get; init; }
        public RetrieveRecurringTransactionsQuery(Guid userId)
        {
            UserId = userId;
        }
    }

    public class RetrieveRecurringTransactionsQueryHandler : IRequestHandler<RetrieveRecurringTransactionsQuery, IEnumerable<RecurringTransactionDto>>
    {
        private readonly BudgetDbContext _dbContext;

        public RetrieveRecurringTransactionsQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<RecurringTransactionDto>> Handle(RetrieveRecurringTransactionsQuery request, CancellationToken cancellationToken)
        {
            var recurringTransactions = await _dbContext.RecurringTransactions
                                    .Where(x => x.UserId == request.UserId)
                                    .Select(recurringTransaction => new RecurringTransactionDto
                                    {
                                        Id = recurringTransaction.Id,
                                        UserId = recurringTransaction.UserId,
                                        Name = recurringTransaction.Name,
                                        Description = recurringTransaction.Description,
                                        Amount = recurringTransaction.Amount,
                                        TransactionType = recurringTransaction.TransactionType,
                                        StartDate = recurringTransaction.StartDate,
                                        EndDate = recurringTransaction.EndDate,
                                        Frequency = recurringTransaction.Frequency,
                                        Interval = recurringTransaction.Interval,
                                        WeeklyDays = recurringTransaction.WeeklyDays,
                                        MonthlyDay = recurringTransaction.MonthlyDay,
                                        YearlyMonth = recurringTransaction.YearlyMonth,
                                        YearlyDay = recurringTransaction.YearlyDay,
                                        MaxOccurrences = recurringTransaction.MaxOccurrences,
                                    })
                                    .OrderBy(x => x.StartDate)
                                    .ToListAsync(cancellationToken);

            return recurringTransactions;
        }
    }
}