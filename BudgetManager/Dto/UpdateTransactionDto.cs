namespace BudgetManager.Dto
{
    public class UpdateTransactionDto
    {
        public int Id { get; set; }
        public bool IncomeType { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
    }
}
