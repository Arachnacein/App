using BudgetManager.Models;
using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Dto
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public TransactionCategoryEnum Category { get; set; }
    }
}
