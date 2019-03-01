using Panel.BusinessLogic.AuxilliaryMethods;
using Panel.Commands;
using Panel.Interfaces;
using Panel.Models.InputModels;
using Panel.Repositories;
using Panel.UserControls;
using Panel.Validations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Panel.ViewModels.InputViewModels
{
    public class MaintenanceViewModel : ViewModelBase, IViewModel
    {
        public ObservableCollection<GeneratorNameModel> UniqueGeneratorNames { get; set; } = 
            new ObservableCollection<GeneratorNameModel>();

        public ObservableCollection<GeneratorNameModel> UniqueGeneratorNamesUnsorted { get; set; } = 
            new ObservableCollection<GeneratorNameModel>();

        public ObservableCollection<string> ReminderLevels { get; set; } = 
            new ObservableCollection<string>();

        public ObservableCollection<GeneratorScheduler> AllGeneratorSchedules { get; set; } = 
            new ObservableCollection<GeneratorScheduler>();

        public ObservableCollection<GeneratorScheduler> ActiveGeneratorSchedules { get; set; } = 
            new ObservableCollection<GeneratorScheduler>();

        public List<string> UniqueAuthoriserNames { get; set; } = new List<string>();

        public MaintenanceViewModel(UnitOfWork unitOfWork, 
            PasswordControl passwordControl)
        {
            UnitOfWork = unitOfWork;
            PasswordControl = passwordControl;

            UniqueGeneratorNamesUnsorted = unitOfWork.GeneratorInformation
                                                     .GetUniqueGeneratorNames();

            UniqueGeneratorNames = new ObservableCollection<GeneratorNameModel>
                                        (UniqueGeneratorNamesUnsorted
                                        .OrderBy(x => x.GeneratorName));

            AllGeneratorSchedules = unitOfWork.GeneratorScheduler
                                              .GetAllGeneratorSchedules();

            ActiveGeneratorSchedules = unitOfWork.GeneratorScheduler
                                                 .GetActiveGeneratorSchedules();

            UniqueAuthoriserNames = unitOfWork.AuthoriserSetting
                                              .GetAuthorisersFullNames();

            ReminderLevels.Add("Normal");
            ReminderLevels.Add("Elevated");
            ReminderLevels.Add("Extreme");
        }

        public UnitOfWork UnitOfWork { get; set; }
        public PasswordControl PasswordControl { get; set; }
        public DateTime UnschMaintenanceDate { get; set; } = DateTime.Now;
        public DateTime SchMaintenanceDate { get; set; } = DateTime.Now;        
        public DateTime SchMaintenanceStartDate { get; set; } = DateTime.Now;
        public double SchMaintenanceReminderHours { get; set; }
        public string SchMaintenanceSelectedReminderLevel { get; set; }
        public string SchMaintenanceSelectedAuthoriser { get; set; }
        public bool RepeatReminder { get; set; }

        private string _unschMaintenanceComments;
        public string UnschMaintenanceComments
        {
            get => _unschMaintenanceComments;
            set
            {
                _unschMaintenanceComments = value;
                OnPropertyChanged(nameof(UnschMaintenanceComments));
                ValidateMaintenanceCommentsRule
                    .ValidateMaintenanceCommentsAsync(UnschMaintenanceComments)
                    .ContinueWith(t =>
                    {
                        lock (_errors)
                        {
                            _errors.Clear();
                            _errors[nameof(UnschMaintenanceComments)] = t.Result;
                            OnErrorsChanged(nameof(UnschMaintenanceComments));
                        }
                    });
            }
        }

        private double _unschMaintenanceTotalCost;
        public double UnschMaintenanceTotalCost
        {
            get => _unschMaintenanceTotalCost;
            set
            {
                _unschMaintenanceTotalCost = value;
                OnPropertyChanged(nameof(UnschMaintenanceTotalCost));
                ValidateMaintenanceTotalCostRule
                    .ValidateMaintenanceTotalCostAsync(UnschMaintenanceTotalCost)
                    .ContinueWith(t =>
                    {
                        lock (_errors)
                        {
                            _errors.Clear();
                            _errors[nameof(UnschMaintenanceTotalCost)] = t.Result;
                            OnErrorsChanged(nameof(UnschMaintenanceTotalCost));
                        }
                    });
            }
        }

        private string _schMaintenanceComments;
        public string SchMaintenanceComments
        {
            get => _schMaintenanceComments;
            set
            {
                _schMaintenanceComments = value;
                OnPropertyChanged(nameof(SchMaintenanceComments));
                ValidateMaintenanceCommentsRule
                    .ValidateMaintenanceCommentsAsync(SchMaintenanceComments)
                    .ContinueWith(t =>
                    {
                        lock (_errors)
                        {
                            _errors.Clear();
                            _errors[nameof(SchMaintenanceComments)] = t.Result;
                            OnErrorsChanged(nameof(SchMaintenanceComments));
                        }
                    });
            }
        }

        private double _schMaintenanceTotalCost;
        public double SchMaintenanceTotalCost
        {
            get => _schMaintenanceTotalCost;
            set
            {
                _schMaintenanceTotalCost = value;
                OnPropertyChanged(nameof(SchMaintenanceTotalCost));
                ValidateMaintenanceTotalCostRule
                    .ValidateMaintenanceTotalCostAsync(SchMaintenanceTotalCost)
                    .ContinueWith(t =>
                    {
                        lock (_errors)
                        {
                            _errors.Clear();
                            _errors[nameof(SchMaintenanceTotalCost)] = t.Result;
                            OnErrorsChanged(nameof(SchMaintenanceTotalCost));
                        }
                    });
            }
        }

        private string _schMaintenanceSelectedGen;
        public string SchMaintenanceSelectedGen
        {
            get => _schMaintenanceSelectedGen;
            set
            {
                _schMaintenanceSelectedGen = value;
                OnPropertyChanged(nameof(SchMaintenanceSelectedGen));

                SchMaintenanceStartDate = UnitOfWork.GeneratorScheduler
                                                    .GetStartDate
                                                    (SchMaintenanceSelectedGen, 
                                                    AllGeneratorSchedules);

                if (SchMaintenanceStartDate < new DateTime(1900, 12, 01))
                    SchMaintenanceStartDate = DateTime.Now;

                OnPropertyChanged(nameof(SchMaintenanceStartDate));

                SchMaintenanceReminderHours = UnitOfWork.GeneratorScheduler
                                                        .GetReminderInHrs(
                                                        SchMaintenanceSelectedGen, 
                                                        AllGeneratorSchedules);

                OnPropertyChanged(nameof(SchMaintenanceReminderHours));

                try
                {
                    SchMaintenanceSelectedReminderLevel = UnitOfWork
                                                        .GeneratorScheduler
                                                        .GetReminderLevel(
                                                        SchMaintenanceSelectedGen, 
                                                        AllGeneratorSchedules);

                    OnPropertyChanged(nameof(SchMaintenanceSelectedReminderLevel));

                    SchMaintenanceSelectedAuthoriser = UnitOfWork
                                                        .GeneratorScheduler
                                                        .GetAuthoriser(
                                                        SchMaintenanceSelectedGen, 
                                                        AllGeneratorSchedules);

                    OnPropertyChanged(nameof(SchMaintenanceSelectedAuthoriser));
                }
                catch (Exception) { }                
            }
        }
        
        private ICommand _unschMaintenanceCmd;
        public ICommand UnschMaintenanceCmd
        {
            get
            {
                return this._unschMaintenanceCmd ??
                (
                    this._unschMaintenanceCmd = new DelegateCommand
                    (
                        x =>
                        {
                            ComboBox cmbxUnschMaintenance = x as ComboBox;
                            if (cmbxUnschMaintenance.Text == null || 
                            cmbxUnschMaintenance.Text == "")
                            {
                                MessageBox.Show("Select a generator!", 
                                    "Error", 
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }
                            UnitOfWork.GeneratorMaintenance
                                      .AddUnschMaintenance("Unscheduled", 
                                      UnschMaintenanceDate, 
                                      UnschMaintenanceComments, 
                                      UnschMaintenanceTotalCost);

                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                                MessageBox.Show($"Unscheduled maintenance " +
                                    $"record for {cmbxUnschMaintenance.Text} " +
                                    $"added!", 
                                    "Information", 
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Information);
                            return;
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _schMaintenanceCmd;
        public ICommand SchMaintenanceCmd
        {
            get
            {
                return this._schMaintenanceCmd ??
                (
                    this._schMaintenanceCmd = new DelegateCommand
                    (
                        x =>
                        {
                            ComboBox cmbxSchMaintenance = x as ComboBox;
                            if (cmbxSchMaintenance.Text == null || 
                            cmbxSchMaintenance.Text == "")
                            {
                                MessageBox.Show("Select a generator!", 
                                    "Error", 
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }
                            UnitOfWork.GeneratorMaintenance
                                      .AddSchMaintenance(
                                      "Scheduled", 
                                      SchMaintenanceDate,
                                      SchMaintenanceComments, 
                                      SchMaintenanceTotalCost);

                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                                MessageBox.Show($"Scheduled maintenance " +
                                    $"record for " +
                                    $"{cmbxSchMaintenance.Text} added!",
                                    "Information", 
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Information);
                            return;
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _unlockReminderNotificationCmd;
        public ICommand UnlockReminderNotificationCmd
        {
            get
            {
                return this._unlockReminderNotificationCmd ??
                (
                    this._unlockReminderNotificationCmd = new DelegateCommand
                    (
                        x =>
                        {
                            Tuple<StackPanel, 
                                  Expander,
                                  Button> 
                                  stcPnlExpnder = 
                                                (Tuple<StackPanel, 
                                                Expander,
                                                Button>)x;

                            stcPnlExpnder.Item2.Visibility = Visibility.Collapsed;                            
                            stcPnlExpnder.Item1.Visibility = Visibility.Visible;

                            foreach (var item in FindChildren
                                                 .FindVisualChildren<PasswordBox>(
                                                 stcPnlExpnder.Item1))
                            {
                                if(item is PasswordBox)
                                {
                                    Keyboard.Focus(item);
                                    break;
                                }
                            }
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
                                  DatePicker,
                                  TextBox, 
                                  ComboBox, 
                                  ComboBox, 
                                  StackPanel,
                                  Expander, 
                                  Tuple<Button>>  pbdptbcbcbstpnl = (Tuple<PasswordBox, 
                                                                    DatePicker,
                                                                    TextBox, 
                                                                    ComboBox,
                                                                    ComboBox, 
                                                                    StackPanel,
                                                                    Expander, 
                                                                    Tuple<Button>>)x;

                            if (pbdptbcbcbstpnl.Item1.Password == "reminder")
                            {
                                pbdptbcbcbstpnl.Item2.IsHitTestVisible = true;
                                pbdptbcbcbstpnl.Item2.Focusable = true;

                                pbdptbcbcbstpnl.Item4.IsHitTestVisible = true;
                                pbdptbcbcbstpnl.Item4.Focusable = true;

                                pbdptbcbcbstpnl.Item5.IsHitTestVisible = true;
                                pbdptbcbcbstpnl.Item5.Focusable = true;

                                pbdptbcbcbstpnl.Item3.IsReadOnly = false;

                                pbdptbcbcbstpnl.Rest.Item1.IsEnabled = true;


                                MessageBoxResult result =  MessageBox.Show("Reminder & " +
                                                                "Notification " +
                                                                "have been activated", 
                                                                "Information",
                                                                MessageBoxButton.OK,
                                                                MessageBoxImage.Information);
                                switch (result)
                                {
                                    case MessageBoxResult.None:
                                    case MessageBoxResult.OK:
                                    case MessageBoxResult.Cancel:
                                    case MessageBoxResult.Yes:
                                    case MessageBoxResult.No:
                                        pbdptbcbcbstpnl.Item6.Visibility = Visibility.Collapsed;
                                        pbdptbcbcbstpnl.Item7.Visibility = Visibility.Visible;
                                        break;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Wrong password", 
                                    "Error",
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
                            Tuple<StackPanel, 
                                  Expander,
                                  Button> 
                                  stcPnlExpnder =
                                                (Tuple<StackPanel, 
                                                       Expander,
                                                       Button>)x;

                            stcPnlExpnder.Item1.Visibility = Visibility.Collapsed;
                            stcPnlExpnder.Item2.Visibility = Visibility.Visible;
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

                            if(SchMaintenanceSelectedGen == null)
                            {
                                MessageBox.Show($"Please select a generator",
                                                "Information",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Information);
                                return;
                            }

                            UnitOfWork.GeneratorScheduler
                                      .ActivateReminderNotification(SchMaintenanceSelectedGen, 
                                                                    SchMaintenanceStartDate, 
                                                                    SchMaintenanceReminderHours,
                                                                    ReminderLevel, 
                                                                    RepeatReminderYesNo, 
                                                                    Authoriser);
                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                            {
                                MessageBox.Show($"Scheduled maintenance " +
                                                $"reminder information " +
                                                $"for {SchMaintenanceSelectedGen} " +
                                                $"has been updated!",
                                                "Information", 
                                                MessageBoxButton.OK, 
                                                MessageBoxImage.Information);

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
                            UniqueAuthoriserNames = UnitOfWork.AuthoriserSetting
                                                              .GetAuthorisersFullNames();

                            OnPropertyChanged(nameof(UniqueAuthoriserNames));
                        },
                        y => !HasErrors
                    )
                );
            }
        }        
    }
}
