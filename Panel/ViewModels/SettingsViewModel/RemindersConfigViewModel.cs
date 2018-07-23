using Panel.Commands;
using Panel.Interfaces;
using Panel.Models.InputModels;
using Panel.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Panel.ViewModels.SettingsViewModel
{
    public class RemindersConfigViewModel : ViewModelBase, IViewModel
    {
        public UnitOfWork UnitOfWork { get; set; }

        public ObservableCollection<GeneratorScheduler> AllGeneratorSchedules { get; set; } =
            new ObservableCollection<GeneratorScheduler>();
        public ObservableCollection<GeneratorScheduler> ActiveGeneratorSchedules { get; set; } =
            new ObservableCollection<GeneratorScheduler>();
        public ObservableCollection<GeneratorNameModel> UniqueGeneratorNames { get; set; } =
            new ObservableCollection<GeneratorNameModel>();
        public ObservableCollection<GeneratorNameModel> UniqueGeneratorNamesUnsorted { get; set; } =
            new ObservableCollection<GeneratorNameModel>();
        public ObservableCollection<string> ReminderLevels { get; set; } =
            new ObservableCollection<string>();


        public RemindersConfigViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;

            ActiveGeneratorSchedules = unitOfWork.GeneratorScheduler
                                                 .GetActiveGeneratorSchedules();

            UniqueGeneratorNamesUnsorted = unitOfWork.GeneratorInformation
                                                     .GetUniqueGeneratorNames();

            UniqueGeneratorNames = new ObservableCollection<GeneratorNameModel>
                (UniqueGeneratorNamesUnsorted.OrderBy(x => x.GeneratorName));

            UniqueAuthoriserFullNames = unitOfWork.AuthoriserSetting
                                                  .GetAuthorisersFullNames();

            ReminderLevels.Add("Normal");
            ReminderLevels.Add("Elevated");
            ReminderLevels.Add("Extreme");
        }

        public DateTime SchMaintenanceStartDate { get { return DateTime.Now; } set { } }
        public double SchMaintenanceReminderHours { get; set; }
        public string SchMaintenanceSelectedReminderLevel { get; set; }
        public List<string> UniqueAuthoriserFullNames { get; set; } = new List<string>();
        public string SchMaintenanceSelectedAuthoriser { get; set; }
        public bool RepeatReminder { get; set; }

        private string _schMaintenanceSelectedGen;
        public string SchMaintenanceSelectedGen
        {
            get => _schMaintenanceSelectedGen;
            set
            {
                _schMaintenanceSelectedGen = value;
                OnPropertyChanged(nameof(SchMaintenanceSelectedGen));
            }
        }

        private ICommand _addReminderNotificationCmd;
        public ICommand AddReminderNotificationCmd
        {
            get
            {
                return this._addReminderNotificationCmd ??
                (
                    this._addReminderNotificationCmd = new DelegateCommand
                    (
                        x =>
                        {
                            string ReminderLevel = SchMaintenanceSelectedReminderLevel;
                            string Authoriser = SchMaintenanceSelectedAuthoriser;

                            string RepeatReminderYesNo = RepeatReminder ? "Yes" : "No";
                            string AuthoriserFirstName = Regex.Match(Authoriser, @"\b[A-Za-z]+\b", 
                                                                RegexOptions.Singleline).Value;
                            UnitOfWork.GeneratorScheduler
                                      .ActivateReminderNotification(SchMaintenanceSelectedGen,
                                                            SchMaintenanceStartDate, 
                                                            SchMaintenanceReminderHours,
                                                            ReminderLevel, RepeatReminderYesNo,
                                                            AuthoriserFirstName);

                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                            {
                                MessageBox.Show($"Scheduled maintenance reminder information " +
                                    $"for {SchMaintenanceSelectedGen} has been updated!",
                                    "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                                ActiveGeneratorSchedules = UnitOfWork.GeneratorScheduler
                                                                  .GetActiveGeneratorSchedules();

                                OnPropertyChanged(nameof(ActiveGeneratorSchedules));
                            }
                            return;
                        },
                        y => !HasErrors
                    )
                );
            }
        }
    }
}
