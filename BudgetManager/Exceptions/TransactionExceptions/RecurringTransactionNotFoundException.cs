namespace BudgetManager.Exceptions.TransactionExceptions
{
    public class RecurringTransactionNotFoundException : Exception
    {
        public RecurringTransactionNotFoundException(string? message) : base(message)
        {
        }
    }
}