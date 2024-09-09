using BudgetManager.Models;

namespace BudgetManager.Dto
{
    public class UpdateTransactionCategoryDto
    {
        public int Id { get; set; }
        public TransactionCategoryEnum  Category { get; set; }
    }
}
