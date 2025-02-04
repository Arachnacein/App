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
                                        Approved = recurringTransaction.Approved,
                                        ScheduleId = recurringTransaction.ScheduleId,
                                        Schedule = new RecurringTransactionScheduleDto
                                        {
                                            Id = recurringTransaction.Schedule.Id,
                                            Frequency = recurringTransaction.Schedule.Frequency,
                                            Interval = recurringTransaction.Schedule.Interval,
                                            WeeklyDays = recurringTransaction.Schedule.WeeklyDays,
                                            MonthlyDay = recurringTransaction.Schedule.MonthlyDay,
                                            YearlyMonth = recurringTransaction.Schedule.YearlyMonth,
                                            YearlyDay = recurringTransaction.Schedule.YearlyDay,
                                            MaxOccurrences = recurringTransaction.Schedule.MaxOccurrences,
                                            RecurringTransactionId = recurringTransaction.Schedule.RecurringTransactionId
                                        }
                                    })
                                    .OrderBy(x => x.StartDate)
                                    .ToListAsync(cancellationToken);

            return recurringTransactions;
        }
    }
}