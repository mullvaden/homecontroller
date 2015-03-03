using System;
using System.Collections.Generic;
using System.Text;

namespace HomeController.TelldusIntegration.Dtos
{
    public static class Utils
    {
        public static StringBuilder ToStringy(this IEnumerable<TemperatureSensor> sensors)
        {
            var sb = new StringBuilder();

            foreach (var sensor in sensors)
                sb.AppendLine(sensor.ToString());
            return sb;
        }

        public static string ToSince(this DateTime dateTime)
        {
            return  dateTime.DaysToPlural() + dateTime.HoursToPlural() + dateTime.MinutesToPlural() + " sedan";
        }

        public static string DaysToPlural(this DateTime dateTime)
        {
            var days = (DateTime.Now - dateTime).Days;
            var hours = (int)(DateTime.Now - dateTime).TotalHours % 24;

            if (days == 0)
                return "";
            var och = days > 0 && hours > 0 ? ", " :" och ";
            return days + " dag" + (days > 1 ? "ar" : "") + och;
        }
        public static string HoursToPlural(this DateTime dateTime)
        {
            var hours = (int)(DateTime.Now - dateTime).TotalHours % 24;
            if (hours == 0)
                return "";
            return hours + " timm" + (hours > 1 ? "ar" : "e") + " och ";
        }
        public static string MinutesToPlural(this DateTime dateTime)
        {
            var minutes = (int)(DateTime.Now - dateTime).TotalMinutes % 60;

            if ((DateTime.Now - dateTime).TotalSeconds < 60)
                return "mindre än en minut";

            if (minutes == 0 && (DateTime.Now - dateTime).TotalMinutes > 60)
                return "0 minuter";
            if (minutes == 0)
                return "";
            return minutes + " minut" + (minutes > 1 ? "er" : "");
        }
    }
}
