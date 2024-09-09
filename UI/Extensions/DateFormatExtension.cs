﻿namespace UI.Extensions
{
    public static class DateFormatExtension
    {
        public static string Format(this DateTime? dateTime)
        {
            return dateTime?.ToString("dd-MM-yyyy");
        }

    }
}