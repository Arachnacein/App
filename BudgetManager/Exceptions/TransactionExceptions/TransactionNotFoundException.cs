namespace BudgetManager.Exceptions.TransactionExceptions
{
    public class TransactionNotFoundException : Exception
    {
        public TransactionNotFoundException(string msg) : base(msg)
        {
            
        }
    }
}
