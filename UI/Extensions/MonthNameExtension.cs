using System.Globalization;

namespace UI.Extensions
{
    public static class MonthNameExtension
    {
        public static string GetMonthName(this DateTime date) => 
            date.ToString("MMMM", CultureInfo.CurrentCulture);
    }
}