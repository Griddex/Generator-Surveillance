using Panel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.BusinessLogic.ChartsLogic.GeneratorChartLogic
{
    public static class GetOrdinateData
    {
        public static List<List<double>> GetData(string SelectedGeneratorName, string SelectedChartType, 
                                            string DurationPerspective, List<string> lstBoxSelectedStringValues)
        {
            List<List<double>> ListOfHoursList = new List<List<double>>();
            CalculateUsageHours calculateUsageHours = new CalculateUsageHours();
            switch (DurationPerspective)
            {
                case "Day":
                    if (SelectedChartType.Contains("Stacked"))
                    {
                        ListOfHoursList.Add(calculateUsageHours.GetGeneratorHoursByDay(SelectedGeneratorName, lstBoxSelectedStringValues));
                        ListOfHoursList.Add(calculateUsageHours.GetPowerHoursByDay(SelectedGeneratorName, lstBoxSelectedStringValues));
                    }
                    else
                    {
                        ListOfHoursList.Add(calculateUsageHours.GetGeneratorHoursByDay(SelectedGeneratorName, lstBoxSelectedStringValues));
                    }
                    return ListOfHoursList;
                case "Week":
                    if (SelectedChartType.Contains("Stacked"))
                    {
                        ListOfHoursList.Add(calculateUsageHours.GetGeneratorHoursByWeek(SelectedGeneratorName, lstBoxSelectedStringValues));
                        ListOfHoursList.Add(calculateUsageHours.GetPowerHoursByWeek(SelectedGeneratorName, lstBoxSelectedStringValues));
                    }
                    else
                    {
                        ListOfHoursList.Add(calculateUsageHours.GetGeneratorHoursByWeek(SelectedGeneratorName, lstBoxSelectedStringValues));
                    }
                    return ListOfHoursList;
                case "Month":
                    if (SelectedChartType.Contains("Stacked"))
                    {
                        ListOfHoursList.Add(calculateUsageHours.GetGeneratorHoursByMonth(SelectedGeneratorName, lstBoxSelectedStringValues));
                        ListOfHoursList.Add(calculateUsageHours.GetPowerHoursByMonth(SelectedGeneratorName, lstBoxSelectedStringValues));
                    }
                    else
                    {
                        ListOfHoursList.Add(calculateUsageHours.GetGeneratorHoursByMonth(SelectedGeneratorName, lstBoxSelectedStringValues));
                    }
                    return ListOfHoursList;
                case "Quarter":
                    if (SelectedChartType.Contains("Stacked"))
                    {
                        ListOfHoursList.Add(calculateUsageHours.GetGeneratorHoursByQuarter(SelectedGeneratorName, lstBoxSelectedStringValues));
                        ListOfHoursList.Add(calculateUsageHours.GetPowerHoursByQuarter(SelectedGeneratorName, lstBoxSelectedStringValues));
                    }
                    else
                    {
                        ListOfHoursList.Add(calculateUsageHours.GetGeneratorHoursByQuarter(SelectedGeneratorName, lstBoxSelectedStringValues));
                    }
                    return ListOfHoursList;
                case "Year":
                    if (SelectedChartType.Contains("Stacked"))
                    {
                        ListOfHoursList.Add(calculateUsageHours.GetGeneratorHoursByYear(SelectedGeneratorName, lstBoxSelectedStringValues));
                        ListOfHoursList.Add(calculateUsageHours.GetPowerHoursByYear(SelectedGeneratorName, lstBoxSelectedStringValues));
                    }
                    else
                    {
                        ListOfHoursList.Add(calculateUsageHours.GetGeneratorHoursByYear(SelectedGeneratorName, lstBoxSelectedStringValues));
                    }
                    return ListOfHoursList;
                default:
                    break;
            }
            return null;
        }
    }
}
