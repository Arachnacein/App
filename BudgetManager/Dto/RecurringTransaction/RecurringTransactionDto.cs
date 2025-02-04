using BudgetManager.Models;

namespace BudgetManager.Dto.RecurringTransaction
{
    public class RecurringTransactionDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Amount { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Approved { get; set; } = false;
        public int ScheduleId { get; set; }
        public RecurringTransactionScheduleDto Schedule { get; set; }
    }
}