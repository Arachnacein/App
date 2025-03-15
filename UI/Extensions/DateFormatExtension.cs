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
        /// <summary>
        /// Return a month name from a month number
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string GetMonthName(this int? month)
        {
            return month switch
            {
                1 => "January",
                2 => "February",
                3 => "March",
                4 => "April",
                5 => "May",
                6 => "June",
                7 => "July",
                8 => "August",
                9 => "September",
                10 => "October",
                11 => "November",
                12 => "December",
                _ => "Invalid month"
            };
        }
    }
}