namespace BudgetManager.Exceptions.PatternExceptions;

public class PatternAlreadyExistsException : Exception
{
    public PatternAlreadyExistsException(string msg) : base(msg) { }
}
