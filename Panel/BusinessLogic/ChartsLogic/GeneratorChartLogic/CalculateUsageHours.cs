using Panel.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.BusinessLogic.ChartsLogic.GeneratorChartLogic
{
    public  class CalculateUsageHours
    {
        public UnitOfWork UnitOfWork { get; set; }

        public List<double> GetGeneratorHoursByDay(string SelectedGeneratorName, List<string> lstBoxSelectedStringValues)
        {
            List<double> HoursByDayList = new List<double>();
            foreach (var dateString in lstBoxSelectedStringValues)
            {
                HoursByDayList.Add(UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                            .Where(x => SelectedGeneratorName == x.GeneratorName)
                                            .Where(x => x.Date.ToString() == dateString)
                                            .Select(x => (x.GeneratorStopped - x.GeneratorStarted).TotalHours)
                                            .Aggregate((x, y) => x + y));
            }
            return HoursByDayList;
        }

        public List<double> GetPowerHoursByDay(string SelectedGeneratorName, List<string> lstBoxSelectedStringValues)
        {
            List<double> HoursByDayList = new List<double>();
            foreach (var dateString in lstBoxSelectedStringValues)
            {
                HoursByDayList.Add(24.0 - (UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                            .Where(x => SelectedGeneratorName == x.GeneratorName)
                                            .Where(x => x.Date.ToString() == dateString)
                                            .Select(x => (x.GeneratorStopped - x.GeneratorStarted).TotalHours)
                                            .Aggregate((x, y) => x + y)));
            }
            return HoursByDayList;
        }


        public List<double> GetGeneratorHoursByWeek(string SelectedGeneratorName, List<string> lstBoxSelectedStringValues)
        {
            List<double> HoursByWeekList = new List<double>();
            foreach (var dateString in lstBoxSelectedStringValues)
            {
                HoursByWeekList.Add(UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                            .Where(x => SelectedGeneratorName == x.GeneratorName)
                                            .Where(x => $"Week {CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(x.Date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday)}, {x.Date.Year}" == dateString)
                                            .Select(x => (x.GeneratorStopped - x.GeneratorStarted).TotalHours)
                                            .Aggregate((x, y) => x + y));
            }
            return HoursByWeekList;
        }

        public List<double> GetPowerHoursByWeek(string SelectedGeneratorName, List<string> lstBoxSelectedStringValues)
        {
            List<double> HoursByWeekList = new List<double>();
            foreach (var dateString in lstBoxSelectedStringValues)
            {
                HoursByWeekList.Add(168 - (UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                            .Where(x => SelectedGeneratorName == x.GeneratorName)
                                            .Where(x => $"Week {CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(x.Date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday)}, {x.Date.Year}" == dateString)
                                            .Select(x => (x.GeneratorStopped - x.GeneratorStarted).TotalHours)
                                            .Aggregate((x, y) => x + y)));
            }
            return HoursByWeekList;
        }

        public List<double> GetGeneratorHoursByMonth(string SelectedGeneratorName, List<string> lstBoxSelectedStringValues)
        {
            List<double> HoursByMonthList = new List<double>();
            foreach (var dateString in lstBoxSelectedStringValues)
            {
                HoursByMonthList.Add(UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                            .Where(x => SelectedGeneratorName == x.GeneratorName)
                                            .Where(x => x.Date.ToString("MMMM, yyyy") == dateString)
                                            .Select(x => (x.GeneratorStopped - x.GeneratorStarted).TotalHours)
                                            .Aggregate((x, y) => x + y));
            }
            return HoursByMonthList;
        }

        public List<double> GetPowerHoursByMonth(string SelectedGeneratorName, List<string> lstBoxSelectedStringValues)
        {
            List<double> HoursByMonthList = new List<double>();
            foreach (var dateString in lstBoxSelectedStringValues)
            {
                HoursByMonthList.Add(730.001 - (UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                            .Where(x => SelectedGeneratorName == x.GeneratorName)
                                            .Where(x => x.Date.ToString("MMMM, yyyy") == dateString)
                                            .Select(x => (x.GeneratorStopped - x.GeneratorStarted).TotalHours)
                                            .Aggregate((x, y) => x + y)));
            }
            return HoursByMonthList;
        }

        public List<double> GetGeneratorHoursByQuarter(string SelectedGeneratorName, List<string> lstBoxSelectedStringValues)
        {
            List<double> HoursByQuarterList = new List<double>();
            foreach (var dateString in lstBoxSelectedStringValues)
            {
                HoursByQuarterList.Add(UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                            .Where(x => SelectedGeneratorName == x.GeneratorName)
                                            .Where(x => $"Q{Convert.ToInt32(x.Date.Month + 2) / 3}, {x.Date.ToString("yyyy")}" == dateString)
                                            .Select(x => (x.GeneratorStopped - x.GeneratorStarted).TotalHours)
                                            .Aggregate((x, y) => x + y));
            }
            return HoursByQuarterList;
        }

        public List<double> GetPowerHoursByQuarter(string SelectedGeneratorName, List<string> lstBoxSelectedStringValues)
        {
            List<double> HoursByQuarterList = new List<double>();
            foreach (var dateString in lstBoxSelectedStringValues)
            {
                HoursByQuarterList.Add(2190.003 - (UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                            .Where(x => SelectedGeneratorName == x.GeneratorName)
                                            .Where(x => $"Q{Convert.ToInt32(x.Date.Month + 2) / 3}, {x.Date.ToString("yyyy")}" == dateString)
                                            .Select(x => (x.GeneratorStopped - x.GeneratorStarted).TotalHours)
                                            .Aggregate((x, y) => x + y)));
            }
            return HoursByQuarterList;
        }


        public List<double> GetGeneratorHoursByYear(string SelectedGeneratorName, List<string> lstBoxSelectedStringValues)
        {
            List<double> HoursByYearList = new List<double>();
            foreach (var dateString in lstBoxSelectedStringValues)
            {
                HoursByYearList.Add(UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                            .Where(x => SelectedGeneratorName == x.GeneratorName)
                                            .Where(x => x.Date.Year.ToString() == dateString)
                                            .Select(x => (x.GeneratorStopped - x.GeneratorStarted).TotalHours)
                                            .Aggregate((x, y) => x + y));
            }
            return HoursByYearList;
        }

        public List<double> GetPowerHoursByYear(string SelectedGeneratorName, List<string> lstBoxSelectedStringValues)
        {
            List<double> HoursByYearList = new List<double>();
            foreach (var dateString in lstBoxSelectedStringValues)
            {
                HoursByYearList.Add(8760 - (UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                            .Where(x => SelectedGeneratorName == x.GeneratorName)
                                            .Where(x => x.Date.Year.ToString() == dateString)
                                            .Select(x => (x.GeneratorStopped - x.GeneratorStarted).TotalHours)
                                            .Aggregate((x, y) => x + y)));
            }
            return HoursByYearList;
        }
    }
}
