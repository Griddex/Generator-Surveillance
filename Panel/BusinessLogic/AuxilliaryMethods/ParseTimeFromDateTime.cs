using System;

namespace Panel.BusinessLogic.AuxilliaryMethods
{
    public static class ParseTimeFromDateTime
    {
        public static string GetTimePart(DateTime dateTime, string TimePart)
        {
            string DateTimeString = dateTime.ToString("hh:mm:ss tt");
            string[] timeparts = DateTimeString.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            switch (TimePart)
            {
                case "Hours":
                    return timeparts[0];
                case "Minutes":
                    return timeparts[1];
                case "Seconds":
                    return timeparts[2];
                case "AMPM":
                    return timeparts[3];
                default:
                    return null;
            }
        }
    }
}
