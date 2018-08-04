using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Repositories
{
    public class ReminderSettingRepository : Repository<GeneratorScheduler>,
        IReminderSettingsRepository
    {
        public ReminderSettingRepository(GeneratorSurveillanceDBEntities context)
            : base(context) { }

        public GeneratorSurveillanceDBEntities GeneratorSurveillanceDBContext
        {
            get { return Context as GeneratorSurveillanceDBEntities; }
        }

        public void SetRepeatReminder(string GeneratorName)
        {
            foreach (var item in GeneratorSurveillanceDBContext
                                  .GeneratorSchedulers
                                  .Where(x => x.Id >= 0 && x.IsActive == "Yes")
                                  .GroupBy(x => x.GeneratorName,
                                  (Key, g) => g.FirstOrDefault()))
            {
                if (item.GeneratorName == GeneratorName)
                {
                    item.IsRepetitive = "Yes";
                    break;
                }
            }           
        }

        public void DeactivateRepeatReminder(string GeneratorName)
        {
            foreach (var item in GeneratorSurveillanceDBContext
                                  .GeneratorSchedulers
                                  .Where(x => x.Id >= 0 && x.IsActive == "Yes")
                                  .GroupBy(x => x.GeneratorName,
                                  (Key, g) => g.FirstOrDefault()))
            {
                if (item.GeneratorName == GeneratorName)
                {
                    item.IsRepetitive = "No";
                    break;
                }
            }
        }
    }
}
