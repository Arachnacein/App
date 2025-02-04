using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models
{
    public class RecurringTransactionSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public FrequencyEnum Frequency { get; set; }

        [Required]
        public int Interval { get; set; }

        public List<DayOfWeek>? WeeklyDays { get; set; }

        public int? MonthlyDay { get; set; }

        public int? YearlyMonth { get; set; }
        public int? YearlyDay { get; set; }

        public int? MaxOccurrences { get; set; }


        public int RecurringTransactionId { get; set; }
        public RecurringTransaction RecurringTransaction { get; set; }
    }
}