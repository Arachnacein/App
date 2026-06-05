namespace UI;

public enum RecurringTransactionTheme { Border = 0, RightBar = 1, Icons = 2 }

public class GlobalInfoClass
{
    public bool IsDarkMode { get; set; } = true;
    public bool ShowLogoutTimer { get; set; } = false;
    public RecurringTransactionTheme RecurringTheme { get; set; } = RecurringTransactionTheme.Icons;
}