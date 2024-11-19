
namespace BudgetManager.Dto.MonthPattern
{
    public class MonthPatternDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public int PatternId { get; set; }
    }
}