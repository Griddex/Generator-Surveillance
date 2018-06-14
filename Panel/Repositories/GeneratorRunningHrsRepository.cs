using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Repositories
{
    public class GeneratorRunningHrsRepository : Repository<GeneratorRunningHr>, IGeneratorRunningHrsRepository
    {
        public GeneratorRunningHrsRepository(GeneratorSurveillanceDBEntities context) : base(context)
        {

        }

        public GeneratorSurveillanceDBEntities GeneratorSurveillanceDBContext
        {
            get { return Context as GeneratorSurveillanceDBEntities; }
        }

        //public void AddReminderNotification(string GeneratorName, double Reminder, double Notification)
        //{
        //    int NoOfRecords = GeneratorSurveillanceDBContext.GeneratorRunningHrs.Count();
        //    GeneratorSurveillanceDBContext.GeneratorRunningHrs.Add
        //    (
        //        new GeneratorRunningHr
        //        {
        //            Id = NoOfRecords,
        //            Generator = GeneratorName,
        //            Date = RunningHoursDate,
        //            Hours = RunningHours
        //        }
        //    );
        //}

        public ObservableCollection<GeneratorRunningHr> GetAllRunningHours()
        {
            var AllGeneratorRunningHours = new ObservableCollection<GeneratorRunningHr>
                                    (
                                    GeneratorSurveillanceDBContext.GeneratorRunningHrs
                                    .AsParallel<GeneratorRunningHr>()
                                    );
            return AllGeneratorRunningHours;
        }
    }
}
