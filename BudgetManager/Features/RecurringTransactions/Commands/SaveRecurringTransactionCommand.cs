using BudgetManager.Dto.RecurringTransaction;
using BudgetManager.Mappers;
using BudgetManager.Models;
using BudgetManager.Services;
using MediatR;

namespace BudgetManager.Features.RecurringTransactions.Commands
{
    public record SaveRecurringTransactionCommand : IRequest<RecurringTransactionDto>
    {
        public Guid UserId { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public double Amount { get; init; }
        public TransactionTypeEnum TransactionType { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public FrequencyEnum Frequency { get; init; }
        public int Interval { get; init; }
        public List<DayOfWeek>? WeeklyDays { get; init; }
        public int? MonthlyDay { get; init; }
        public int? YearlyMonth { get; init; }
        public int? YearlyDay { get; init; }
        public int? MaxOccurrences { get; init; }

        public SaveRecurringTransactionCommand(Guid userId, string name, string? description, double amount, 
                            TransactionTypeEnum transactionType, DateTime startDate, DateTime endDate, 
                            FrequencyEnum frequency, int interval, List<DayOfWeek>? weeklyDays, int? monthlyDay, 
                            int? yearlyMonth, int? yearlyDay, int? maxOccurrences)
        {
            UserId = userId;
            Name = name;
            Description = description;
            Amount = amount;
            TransactionType = transactionType;
            StartDate = startDate;
            EndDate = endDate;
            Frequency = frequency;
            Interval = interval;
            WeeklyDays = weeklyDays;
            MonthlyDay = monthlyDay;
            YearlyMonth = yearlyMonth;
            YearlyDay = yearlyDay;
            MaxOccurrences = maxOccurrences;
        }
    }

    public class SaveRecurringTransactionCommandHandler : IRequestHandler<SaveRecurringTransactionCommand, RecurringTransactionDto>
    {
        private readonly IRecurringTransactionService _recurringTransactionService;
        private readonly IRecurringTransactionMapper _recurringTransactionMapper;
        public SaveRecurringTransactionCommandHandler(IRecurringTransactionService recurringTransactionService, 
                                                      IRecurringTransactionMapper recurringTransactionMapper)
        {
            _recurringTransactionService = recurringTransactionService;
            _recurringTransactionMapper = recurringTransactionMapper;
        }
        public async Task<RecurringTransactionDto> Handle(SaveRecurringTransactionCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            return await _recurringTransactionService.AddRecurringTransactionAsync(_recurringTransactionMapper.Map(request));
        }
    }
}