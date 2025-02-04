using BudgetManager.Models;

namespace BudgetManager.Dto.RecurringTransaction
{
    public class RecurringTransactionScheduleDto
    {
        public int Id { get; set; }
        public FrequencyEnum Frequency { get; set; }
        public int Interval { get; set; }
        public List<DayOfWeek>? WeeklyDays { get; set; }
        public int? MonthlyDay { get; set; }
        public int? YearlyMonth { get; set; }
        public int? YearlyDay { get; set; }
        public int? MaxOccurrences { get; set; }
        public int RecurringTransactionId { get; set; }
    }
}