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
        public bool Approved { get; set; } = false;

        public int ScheduleId { get; set; }
        public RecurringTransactionSchedule Schedule { get; set; }
    }
}