using Panel.Interfaces;
using Panel.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.ViewModels.TableViewModels
{
    public class RunningHrsSchedulingTablesViewModel : ViewModelBase, IViewModel
    {
        public RunningHrsSchedulingTablesViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            AllRunningHoursRecords = UnitOfWork.GeneratorRunningHr.GetAllRunningHours();
            AllScheduledReminderRecords = UnitOfWork.GeneratorScheduler.GetAllScheduledReminders();
        }

        public UnitOfWork UnitOfWork { get; set; }
        public ObservableCollection<GeneratorRunningHr> AllRunningHoursRecords { get; }
        public ObservableCollection<GeneratorScheduler> AllScheduledReminderRecords { get; }
    }
}
