using BudgetManager.Dto.Pattern;

namespace BudgetManager.Dto.MonthPattern
{
    public class FullMonthPatternDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public PatternDto Pattern { get; set; }
    }
}
