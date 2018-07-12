using Panel.Interfaces;
using Panel.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace Panel.BusinessLogic.MaintenanceLogic
{
    public class ScheduledRemindersMethods
    {
        public static ObservableCollection<GeneratorScheduler> AllGeneratorSchedules { get; private set; }
        public static GeneratorScheduler NextGeneratorForNotification { get; private set; }        
        public static UnityContainer container { get; private set; } = (UnityContainer)Application.Current.Resources["UnityIoC"];
        public static IUnitOfWork UnitOfWork = container.Resolve<IUnitOfWork>("UnitOfWork");

        public static void DeactivateGenerator(string GeneratorName)
        {
            var gse = container.Resolve<GeneratorSurveillanceDBEntities>();       

            foreach (var item in gse.GeneratorSchedulers.Where(x => x.IsActive == "Yes"))
            {
                if (item.GeneratorName == GeneratorName)
                {
                    item.IsActive = "No";
                }
            }

            int Success = UnitOfWork.Complete();
            if (Success > 0)
                MessageBox.Show($"{GeneratorName} deactivated!",
                    "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }        
    }
}
