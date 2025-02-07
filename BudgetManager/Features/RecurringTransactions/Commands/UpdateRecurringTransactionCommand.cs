using BudgetManager.Mappers;
using BudgetManager.Models;
using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.RecurringTransactions.Commands
{
    public record UpdateRecurringTransactionCommand : IRequest
    {
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public double Amount { get; init; }
        public TransactionTypeEnum TransactionType { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public bool Approved { get; init; } = false;
        public int ScheduleId { get; init; }
        public RecurringTransactionSchedule Schedule { get; init; }

        public UpdateRecurringTransactionCommand(int id, Guid userId, string name, string? description,
                                               double amount, TransactionTypeEnum transactionType,
                                               DateTime startDate, DateTime endDate,
                                               bool approved, int scheduleId, RecurringTransactionSchedule schedule)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Description = description;
            Amount = amount;
            TransactionType = transactionType;
            StartDate = startDate;
            EndDate = endDate;
            Approved = approved;
            ScheduleId = scheduleId;
            Schedule = schedule;
        }
    }
    public class UpdateRecurringTransactionCommandHandler : IRequestHandler<UpdateRecurringTransactionCommand>
    {
        private readonly IRecurringTransactionService _recurringTransactionService;
        private readonly IRecurringTransactionMapper _recurringTransactionMapper;
        public UpdateRecurringTransactionCommandHandler(IRecurringTransactionService recurringTransactionService,
                                                      IRecurringTransactionMapper recurringTransactionMapper)
        {
            _recurringTransactionService = recurringTransactionService;
            _recurringTransactionMapper = recurringTransactionMapper;
        }

        public async Task Handle(UpdateRecurringTransactionCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));
            await _recurringTransactionService.UpdateRecurringTransactionAsync(_recurringTransactionMapper.Map(request));
        }
    }
}