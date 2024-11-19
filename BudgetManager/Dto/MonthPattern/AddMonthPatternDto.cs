namespace BudgetManager.Dto.MonthPattern
{
    public class AddMonthPatternDto
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public int PatternId { get; set; }
    }
}