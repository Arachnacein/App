using BudgetManager.Models;

namespace BudgetManager.Dto.RecurringTransaction
{
    public class AddRecurringTransactionDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Amount { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ScheduleId { get; set; }
        public FrequencyEnum Frequency { get; set; }
        public int Interval { get; set; }
        public List<DayOfWeek>? WeeklyDays { get; set; }
        public int? MonthlyDay { get; set; }
        public int? YearlyMonth { get; set; }
        public int? YearlyDay { get; set; }
        public int? MaxOccurrences { get; set; }
    }
}