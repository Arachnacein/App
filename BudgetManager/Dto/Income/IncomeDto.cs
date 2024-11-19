namespace BudgetManager.Dto.Income
{
    public class IncomeDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}