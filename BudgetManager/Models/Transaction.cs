using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool IncomeType { get; set; }

        [MinLength(5)]
        [MaxLength(15)]
        [Required]
        public string Name { get; set; }

        [MinLength(5)]
        [MaxLength(50)]
        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double Price { get; set; }
        [Required]
        public TransactionCategoryEnum Category { get; set; }

    }
}
