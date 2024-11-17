using BudgetManager.Models;

namespace BudgetManager.Dto.Transaction
{
    public class UpdateTransactionDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public TransactionCategoryEnum Category { get; set; }
    }
}
