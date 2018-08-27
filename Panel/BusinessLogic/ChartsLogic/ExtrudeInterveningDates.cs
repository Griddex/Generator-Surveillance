using Panel.Interfaces;
using Panel.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Panel.BusinessLogic
{
    public class ExtrudeInterveningDates : IChartsLogic
    {
        public UnitOfWork UnitOfWork { get; set; }
        public ExtrudeInterveningDates(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        
        public IEnumerable<string> GetInterveningDates(DateTime startdate, DateTime stopdate, string generatorname, string dateperspective = "Day")
        {
            string sysDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            DateTime stDate = startdate.Date;
            DateTime stTime = Convert.ToDateTime($"{default(DateTime).AddDays(693593).ToString(sysDateFormat)} {startdate.TimeOfDay}");
            DateTime spDate = stopdate.Date;
            DateTime spTime = Convert.ToDateTime($"{default(DateTime).AddDays(693593).ToString(sysDateFormat)} {stopdate.TimeOfDay}");

            switch (dateperspective)
            {
                case "Day":
                    var daydates = UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                    .Where(x => generatorname == x.GeneratorName)
                                    .Where(x => x.Date >= stDate && x.Date <= spDate)
                                    .OrderBy(x => x.Date)
                                    .Select(x => x.Date.ToLongDateString()).Distinct();
                    return daydates;

                case "Week":
                    var weekdates = UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                    .Where(x => generatorname == x.GeneratorName)
                                    .Where(x => x.Date >= stDate && x.Date <= spDate)                                    
                                    .OrderBy(x => x.Date)
                                    .Select
                                    (
                                        x => $"Week {CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(x.Date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday)}, {x.Date.Year}"
                                    )
                                    .Distinct();
                    return weekdates;
                case "Month":
                    var monthdates = UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                    .Where(x => generatorname == x.GeneratorName)
                                    .Where(x => x.Date >= stDate && x.Date <= spDate)                                    
                                    .OrderBy(x => x.Date)
                                    .Select(x => x.Date.ToString("MMMM, yyyy"))
                                    .Distinct();
                    return monthdates;
                case "Quarter":
                    var quarterdates = UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                    .Where(x => generatorname == x.GeneratorName)
                                    .Where(x => x.Date >= stDate && x.Date <= spDate)                                    
                                    .OrderBy(x => x.Date)
                                    .Select(x => $"Q{Convert.ToInt32(x.Date.Month + 2)/3}, {x.Date.ToString("yyyy")}")
                                    .Distinct();
                    return quarterdates;
                case "Year":
                    var yeardates = UnitOfWork.GeneratorUsage.GetAllGeneratorUsages()
                                    .Where(x => generatorname == x.GeneratorName)
                                    .Where(x => x.Date >= stDate && x.Date <= spDate)                                    
                                    .OrderBy(x => x.Date)
                                    .Select(x => x.Date.Year.ToString())
                                    .Distinct();
                    return yeardates;
                default:
                    break;
            }
            return null;
        }


    }
}
