namespace BudgetManager.Exceptions.PatternExceptions
{
    public class PatternInUseException : Exception
    {
        public PatternInUseException(string msg) : base(msg) { }
    }
}
