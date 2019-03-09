using Panel.Commands;
using Panel.Interfaces;
using Panel.Models.InputModels;
using Panel.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
                                        (UniqueGeneratorNamesUnsorted
                                        .OrderBy(x => x.GeneratorName));

            UniqueAuthoriserFullNames = unitOfWork.AuthoriserSetting
                                                  .GetAuthorisersFullNames();

            ReminderLevels.Add("Normal");
            ReminderLevels.Add("Elevated");
            ReminderLevels.Add("Extreme");
        }

        public DateTime SchMaintenanceStartDate { get; set; } = DateTime.Now;
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

        private ICommand _uniqueAuthoriserFullNamesCmd;
        public ICommand UniqueAuthoriserFullNamesCmd
        {
            get
            {
                return this._uniqueAuthoriserFullNamesCmd ??
                (
                    this._uniqueAuthoriserFullNamesCmd = new DelegateCommand
                    (
                        x =>
                        {
                            UniqueAuthoriserFullNames = UnitOfWork.AuthoriserSetting
                                                                  .GetAuthorisersFullNames();

                            OnPropertyChanged(nameof(UniqueAuthoriserFullNames));
                        },
                        y => !HasErrors
                    )
                );
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
                            //string AuthoriserFirstName = Regex.Match(Authoriser, @"\b[A-Za-z]+\b", 
                            //                                    RegexOptions.Singleline).Value;

                            string AuthoriserFullName = Authoriser;

                            UnitOfWork.GeneratorScheduler
                                      .ActivateReminderNotification(SchMaintenanceSelectedGen,
                                                            SchMaintenanceStartDate, 
                                                            SchMaintenanceReminderHours,
                                                            ReminderLevel, RepeatReminderYesNo,
                                                            AuthoriserFullName);

                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                            {
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

        private ICommand _refreshCmd;
        public ICommand RefreshCmd
        {
            get
            {
                return this._refreshCmd ??
                (
                    this._refreshCmd = new DelegateCommand
                    (
                        y =>
                        {
                            UniqueGeneratorNamesUnsorted = UnitOfWork.GeneratorInformation
                                                .GetUniqueGeneratorNames();

                            UniqueGeneratorNames = new ObservableCollection<GeneratorNameModel>
                                (UniqueGeneratorNamesUnsorted
                                    .OrderBy(x => x.GeneratorName));

                            OnPropertyChanged(nameof(UniqueGeneratorNames));
                        },
                        z => !HasErrors
                    )
                );
            }
        }

        private ICommand _refreshRemindersTableCmd;
        public ICommand RefreshRemindersTableCmd
        {
            get
            {
                return this._refreshRemindersTableCmd ??
                (
                    this._refreshRemindersTableCmd = new DelegateCommand
                    (
                        x =>
                        {
                             ActiveGeneratorSchedules = UnitOfWork.GeneratorScheduler
                                                                 .GetActiveGeneratorSchedules();
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _deleteRemindersCmd;
        public ICommand DeleteRemindersCmd
        {
            get
            {
                return this._deleteRemindersCmd ??
                (
                    this._deleteRemindersCmd = new DelegateCommand
                    (
                        x =>
                        {
                            MessageBoxResult result = MessageBox.Show(
                                "Do you want to " +
                                "delete all inactive reminders?", 
                                "Confirmation", 
                                MessageBoxButton.YesNoCancel, 
                                MessageBoxImage.Question);

                            switch (result)
                            {
                                case MessageBoxResult.None:
                                case MessageBoxResult.No:
                                case MessageBoxResult.Cancel:
                                    return;
                                case MessageBoxResult.OK:
                                case MessageBoxResult.Yes:
                                    UnitOfWork.GeneratorScheduler
                                              .DeleteInactiveReminders();

                                    int Success = UnitOfWork.Complete();
                                    if (Success > 0)
                                    {
                                        //MessageBox.Show($"All inactive reminders" +
                                        //    $"have been deleted!",
                                        //    "Information", MessageBoxButton.OK,
                                        //    MessageBoxImage.Information);

                                        ActiveGeneratorSchedules = UnitOfWork.GeneratorScheduler
                                                                             .GetActiveGeneratorSchedules();

                                        OnPropertyChanged(nameof(ActiveGeneratorSchedules));
                                    }
                                    break;                                   
                            }                            
                            return;
                        },
                        y => !HasErrors
                    )
                );
            }
        }


        private ICommand _okCmd;
        public ICommand OKCmd
        {
            get
            {
                return this._okCmd ??
                (
                    this._okCmd = new DelegateCommand
                    (
                        x =>
                        {
                                Tuple<PasswordBox, 
                                Expander, 
                                Expander,
                                StackPanel,
                                Viewbox>  expdrexpdr = (Tuple<PasswordBox, 
                                                                Expander,
                                                                Expander,
                                                                StackPanel,
                                                                Viewbox>)x;

                            if (expdrexpdr.Item1.Password == "reminder")
                            {
                                MessageBoxResult result = MessageBox.Show("Reminder & Notification " +
                                                            "settings are unlocked", "Information",
                                                            MessageBoxButton.OKCancel,
                                                            MessageBoxImage.Information);
                                switch (result)
                                {
                                    case MessageBoxResult.None:
                                    case MessageBoxResult.OK:
                                    case MessageBoxResult.Yes:
                                        expdrexpdr.Item4.Visibility = Visibility.Collapsed;
                                        expdrexpdr.Item5.Margin = new Thickness(0, 270, 0, 0);
                                        expdrexpdr.Item5.Visibility = Visibility.Visible;
                                        break;
                                    case MessageBoxResult.Cancel:
                                    case MessageBoxResult.No:                                        
                                        break;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Wrong password", "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                                return;
                            }
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _closeCmd;
        public ICommand CloseCmd
        {
            get
            {
                return this._closeCmd ??
                (
                    this._closeCmd = new DelegateCommand
                    (
                        x =>
                        {
                            Tuple<StackPanel> stcPnlExpnder =
                                            (Tuple<StackPanel>)x;

                            stcPnlExpnder.Item1.Visibility = Visibility.Collapsed;
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _setRepeatReminderCmd;
        public ICommand SetRepeatReminderCmd
        {
            get
            {
                return this._setRepeatReminderCmd ??
                (
                    this._setRepeatReminderCmd = new DelegateCommand
                    (
                        x =>
                        {
                            GeneratorScheduler selectedRow = (GeneratorScheduler)x;

                            UnitOfWork.ReminderSetting.SetRepeatReminder(
                                                selectedRow.GeneratorName);

                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                            {
                                //MessageBox.Show($"Repetitive Reminder" +
                                //    $"activated for {selectedRow.GeneratorName}",
                                //    "Information", 
                                //    MessageBoxButton.OK,
                                //    MessageBoxImage.Information);

                                ActiveGeneratorSchedules = UnitOfWork.GeneratorScheduler
                                                                     .GetActiveGeneratorSchedules();

                                OnPropertyChanged(nameof(ActiveGeneratorSchedules));
                            }
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _deactivateRepeatReminderCmd;
        public ICommand DeactivateRepeatReminderCmd
        {
            get
            {
                return this._deactivateRepeatReminderCmd ??
                (
                    this._deactivateRepeatReminderCmd = new DelegateCommand
                    (
                        x =>
                        {
                            GeneratorScheduler selectedRow = (GeneratorScheduler)x;

                            UnitOfWork.ReminderSetting.DeactivateRepeatReminder(
                                                selectedRow.GeneratorName);

                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                            {

                                ActiveGeneratorSchedules = UnitOfWork.GeneratorScheduler
                                                                     .GetActiveGeneratorSchedules();

                                OnPropertyChanged(nameof(ActiveGeneratorSchedules));

                                //MessageBox.Show($"Repetitive Reminder" +
                                //    $"activated for {selectedRow.GeneratorName}",
                                //    "Information",
                                //    MessageBoxButton.OK,
                                //    MessageBoxImage.Information);
                            }
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _deleteReminderCmd;
        public ICommand DeleteReminderCmd
        {
            get
            {
                return this._deleteReminderCmd ??
                (
                    this._deleteReminderCmd = new DelegateCommand
                    (
                        x =>
                        {
                            GeneratorScheduler selectedRow = (GeneratorScheduler)x;

                            MessageBoxResult result = MessageBox.Show($"You are about to" +
                                                        $" delete all reminder " +
                                                        $"records for {selectedRow.GeneratorName}" +
                                                        $"\n\nContinue?", 
                                                        $"Confirmation",
                                                        MessageBoxButton.YesNoCancel,
                                                        MessageBoxImage.Warning);
                            switch (result)
                            {
                                case MessageBoxResult.None:
                                case MessageBoxResult.OK:
                                case MessageBoxResult.Yes:
                                    UnitOfWork.ReminderSetting.DeleteReminder(
                                                selectedRow.GeneratorName);

                                    int Success = UnitOfWork.Complete();
                                    if (Success > 0)
                                    {
                                        ActiveGeneratorSchedules = UnitOfWork.GeneratorScheduler
                                                                             .GetActiveGeneratorSchedules();

                                        OnPropertyChanged(nameof(ActiveGeneratorSchedules));
                                    }
                                    break;
                                case MessageBoxResult.Cancel:
                                case MessageBoxResult.No:
                                    break;
                            }                   
                        },
                        y => !HasErrors
                    )
                );
            }
        }
    }
}
