using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.BusinessLogic.MaintenanceLogic
{
    public static class ScheduledMaintenanceNotificationLogic
    {
        public static Tuple<List<double>, List<DateTime>> GenerateNotificationHoursAndDates(DateTime StartDate, double EveryHrs, string ReminderLevel)
        {
            List<double> NotificationHours;
            List<DateTime> NotificationDateTime;

            NotificationHours = GenerateNotificationHours(EveryHrs, ReminderLevel);
            NotificationDateTime = GenerateNotificationDateTime(StartDate, NotificationHours);

            return new Tuple<List<double>, List<DateTime>>(NotificationHours, NotificationDateTime);            
        }

        static List<double> GenerateNotificationHours(double EveryHrs, string ReminderLevel)
        {
            List<double> NotificationHours = new List<double>();
            if(ReminderLevel == "Normal")
            {
                for (int i = 1; i < 12; i++)
                {
                    NotificationHours.Add((1 / (Math.Pow(2, i))) * EveryHrs);
                }
            }
            else if(ReminderLevel == "Elevated")
            {
                for (int i = 1; i < 23; i++)
                {
                    NotificationHours.Add((1 / (Math.Pow(3, i))) * EveryHrs);
                }
            }
            else if (ReminderLevel == "Extreme")
            {
                for (int i = 1; i < 45; i++)
                {
                    NotificationHours.Add((1 / (Math.Pow(4, i))) * EveryHrs);
                }
            }
            return NotificationHours;
        }

        static List<DateTime> GenerateNotificationDateTime(DateTime StartDate, List<double> NotificationHoursList)
        {
            double hrs = 0;
            List<DateTime> NotificationDateTime = new List<DateTime>();
            foreach (double hours in NotificationHoursList)
            {
                hrs += hours;
                NotificationDateTime.Add(StartDate.AddHours(hrs));
            }
            return NotificationDateTime;
        }
    }
}
