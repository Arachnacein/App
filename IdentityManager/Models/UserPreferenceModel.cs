namespace IdentityManager.Models;

public class UserPreferenceModel
{
    public string UserId { get; set; } = string.Empty;
    public bool IsDarkMode { get; set; }
    public int RecurringTransactionTheme { get; set; }
}