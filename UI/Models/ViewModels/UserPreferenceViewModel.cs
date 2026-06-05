namespace UI.Models.ViewModels;

public class UserPreferenceViewModel
{
    public Guid UserId { get; set; }
    public bool IsDarkMode { get; set; }
    public int RecurringTransactionTheme { get; set; }
}