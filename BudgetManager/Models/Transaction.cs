using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [MinLength(3)]
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        [MinLength(3)]
        [MaxLength(150)]
        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double Price { get; set; }
        [Required]
        public TransactionCategoryEnum Category { get; set; }
        [Required]
        public bool IsRecurring { get; set; }
        [Required]
        public bool IsApproved { get; set; }

    }
}