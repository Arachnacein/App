using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models
{
    public class MonthPattern
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public int Pattern_Id { get; set; }

        ///
        public Pattern Pattern { get; set; }
    }
}
