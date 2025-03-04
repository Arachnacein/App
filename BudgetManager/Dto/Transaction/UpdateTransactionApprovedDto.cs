namespace BudgetManager.Dto.Transaction
{
    public class UpdateTransactionApprovedDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public bool IsRecurring { get; set; }
        public bool Approved { get; set; }
    }
}