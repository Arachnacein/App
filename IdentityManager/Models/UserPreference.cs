namespace IdentityManager.Models;

public class UserPreference
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public bool IsDarkMode { get; set; } = true;

    public int RecurringTransactionTheme { get; set; } = 1;

    public ApplicationUser User { get; set; } = null!;
}