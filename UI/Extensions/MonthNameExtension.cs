namespace UI.Extensions;

public static class MonthNameExtension
{
    /// <summary>
    ///     Gets the month name from a DateTime object.
    /// </summary>
    /// <param name="date">The DateTime object.</param>
    /// <returns>The month name.</returns>
    public static string GetMonthName(this DateTime date) => 
        date.ToString("MMMM", CultureInfo.CurrentCulture);
}