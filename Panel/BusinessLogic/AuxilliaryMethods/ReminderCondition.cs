using System;
using System.Diagnostics;

namespace Panel.BusinessLogic.AuxilliaryMethods
{
    public static class ReminderCondition
    {
        public static string GetReminderCondition(string ReminderLevel, int FirstID,
                                        int LastID, int GeneratorID)
        {
            double notificationposition = (Convert.ToDouble(GeneratorID) - Convert.ToDouble(FirstID)) /
                                          (Convert.ToDouble(LastID) - Convert.ToDouble(FirstID));
            
            switch (ReminderLevel)
            {
                case "Normal":
                    if (notificationposition <= 0.273)
                        return "Gentle";
                    else if (notificationposition > 0.273 && notificationposition <= 0.545)
                        return "Urgent";
                    else if (notificationposition > 0.545 && notificationposition <= 1)
                        return "Critical";
                    else if (notificationposition > 1)
                        return "Expired";
                    break;
                case "Elevated":
                    if (notificationposition <= 0.136)
                        return "Gentle";
                    else if (notificationposition > 0.136 && notificationposition <= 0.227)
                        return "Urgent";
                    else if (notificationposition > 0.227 && notificationposition <= 1)
                        return "Critical";
                    else if (notificationposition > 1)
                        return "Expired";
                    break;
                case "Extreme":
                    if (notificationposition <= 0.023)
                        return "Gentle";
                    else if (notificationposition > 0.023 && notificationposition <= 0.045)
                        return "Urgent";
                    else if (notificationposition > 0.045 && notificationposition <= 1)
                        return "Critical";
                    else if (notificationposition > 1)
                        return "Expired";
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}
