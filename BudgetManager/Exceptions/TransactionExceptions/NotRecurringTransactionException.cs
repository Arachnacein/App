namespace BudgetManager.Exceptions.TransactionExceptions
{
    public class NotRecurringTransactionException : Exception
    {
        public NotRecurringTransactionException(string? message) : base(message)
        {
        }
    }
}