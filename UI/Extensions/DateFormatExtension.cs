namespace UI.Extensions
{
    public static class DateFormatExtension
    {
        /// <summary>
        /// Format a DateTime object to a string in the format dd-MM-yyyy
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string FormatDMY(this DateTime? dateTime)
        {
            return dateTime?.ToString("dd-MM-yyyy");
        }
        /// <summary>
        /// Format a DateTime object to a string in the format MM-yyyy
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string FormatMY(this DateTime? dateTime)
        {
            return dateTime?.ToString("MM-yyyy");
        }
        /// <summary>
        /// Format a DateTime object to a string in the format dd-MM
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string FormatDM(this DateTime? dateTime)
        {
            return dateTime?.ToString("dd-MM");
        }
    }
}