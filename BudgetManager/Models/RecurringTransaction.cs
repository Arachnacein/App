using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models
{
    public class RecurringTransaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public TransactionTypeEnum TransactionType { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }


        [Required]
        public FrequencyEnum Frequency { get; set; }
        [Required]
        public int Interval { get; set; }
        public List<DayOfWeek>? WeeklyDays { get; set; }
        public int? MonthlyDay { get; set; }
        public int? YearlyMonth { get; set; }
        public int? YearlyDay { get; set; }
        public int? MaxOccurrences { get; set; }
    }
}