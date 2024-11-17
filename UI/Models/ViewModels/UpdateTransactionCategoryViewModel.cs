namespace UI.Models.ViewModels
{
    public class UpdateTransactionCategoryViewModel
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public TransactionCategoryEnum Category { get; set; }
    }
}