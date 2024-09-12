using BudgetManager.Models;

namespace BudgetManager.Dto.Transaction
{
    public class UpdateTransactionCategoryDto
    {
        public int Id { get; set; }
        public TransactionCategoryEnum Category { get; set; }
    }
}
