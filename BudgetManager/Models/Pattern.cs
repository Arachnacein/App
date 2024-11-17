using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models
{
    public class Pattern
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [MinLength(3)]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        public double Value_Saves { get; set; }

        [Required]
        public double Value_Fees { get; set; }

        [Required]
        public double Value_Entertainment { get; set; }

        ///
        public ICollection<MonthPattern> MonthPatterns { get; set; }
    }
}