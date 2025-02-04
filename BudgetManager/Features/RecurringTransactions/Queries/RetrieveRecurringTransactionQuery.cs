using BudgetManager.Data;
using BudgetManager.Dto.RecurringTransaction;
using BudgetManager.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Features.RecurringTransactions.Queries
{
    public record RetrieveRecurringTransactionQuery : IRequest<RecurringTransactionDto>
    {
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public RetrieveRecurringTransactionQuery(int id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
    public class RetrieveRecurringTransactionQueryHandler : IRequestHandler<RetrieveRecurringTransactionQuery, RecurringTransactionDto>
    {
        private readonly BudgetDbContext _dbContext;

        public RetrieveRecurringTransactionQueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RecurringTransactionDto> Handle(RetrieveRecurringTransactionQuery request, CancellationToken cancellationToken)
        {
            var recurringTransaction = await _dbContext.RecurringTransactions
                                    .Where(x => x.Id == request.Id && x.UserId == request.UserId)
                                    .Select(recurringTransaction => new RecurringTransaction
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
                                        Schedule = recurringTransaction.Schedule
                                    })
                                    .FirstOrDefaultAsync(cancellationToken);
            if(recurringTransaction == null)
                throw new ArgumentNullException(nameof(recurringTransaction));

            return recurringTransaction;
        }
    }
}