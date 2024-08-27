using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models
{
    public class Pattern
    {
        [Key]
        public int Id { get; set; }

        [MinLength(5)]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        public double Value_Entertainment { get; set; }

        [Required]
        public double Value_Saves { get; set; }

        [Required]
        public double Value_Fees { get; set; }
    }
}
