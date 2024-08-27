namespace BudgetManager.Dto
{
    public class AddTransactionDto
    {
        public bool IncomeType { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
    }
}
