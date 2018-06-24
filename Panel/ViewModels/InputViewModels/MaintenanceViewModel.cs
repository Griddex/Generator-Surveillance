using Panel.Commands;
using Panel.Interfaces;
using Panel.Models;
using Panel.Models.InputModels;
using Panel.Repositories;
using Panel.Validations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualBasic;

namespace Panel.ViewModels.InputViewModels
{
    public class MaintenanceViewModel : ViewModelBase, IViewModel
    {

        public ObservableCollection<GeneratorNameModel> UniqueGeneratorNames { get; set; } = new ObservableCollection<GeneratorNameModel>();
        public ObservableCollection<string> ReminderLevels { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<GeneratorScheduler> AllGeneratorSchedules { get; set; } = new ObservableCollection<GeneratorScheduler>();
        public ObservableCollection<GeneratorScheduler> ActiveGeneratorSchedules { get; set; } = new ObservableCollection<GeneratorScheduler>();
        public List<string> UniqueAuthorizerNames { get; set; } = new List<string>();

        public MaintenanceViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            UniqueGeneratorNames = unitOfWork.GeneratorInformation.GetUniqueGeneratorNames();
            AllGeneratorSchedules = unitOfWork.GeneratorScheduler.GetAllGeneratorSchedules();
            ActiveGeneratorSchedules = unitOfWork.GeneratorScheduler.GetActiveGeneratorSchedules();
            UniqueAuthorizerNames.Add("Monday");
            UniqueAuthorizerNames.Add("Richard");
            ReminderLevels.Add("Normal");
            ReminderLevels.Add("Elevated");
            ReminderLevels.Add("Extreme");
        }

        public UnitOfWork UnitOfWork { get; set; }
        public DateTime UnschMaintenanceDate { get; set; } = DateTime.Now;
        public DateTime SchMaintenanceDate { get; set; } = DateTime.Now;
        public DateTime SchMaintenanceStartDate
        {
            get
            {
                return DateTime.Now;
            }
            set { }
        } 

        public double SchMaintenanceReminderHours { get; set; }
        public string SchMaintenanceSelectedReminderLevel { get; set; }
        public string SchMaintenanceSelectedAuthorizer { get; set; }
        

        private string _unschMaintenanceComments;
        public string UnschMaintenanceComments
        {
            get => _unschMaintenanceComments;
            set
            {
                _unschMaintenanceComments = value;
                OnPropertyChanged(nameof(UnschMaintenanceComments));
                ValidateMaintenanceCommentsRule.ValidateMaintenanceCommentsAsync(UnschMaintenanceComments)
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
                ValidateMaintenanceTotalCostRule.ValidateMaintenanceTotalCostAsync(UnschMaintenanceTotalCost)
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
                ValidateMaintenanceCommentsRule.ValidateMaintenanceCommentsAsync(SchMaintenanceComments)
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
                ValidateMaintenanceTotalCostRule.ValidateMaintenanceTotalCostAsync(SchMaintenanceTotalCost)
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

                SchMaintenanceStartDate = UnitOfWork.GeneratorScheduler.GetStartDate(SchMaintenanceSelectedGen, AllGeneratorSchedules);
                OnPropertyChanged(nameof(SchMaintenanceStartDate));

                SchMaintenanceReminderHours = UnitOfWork.GeneratorScheduler.GetReminderInHrs(SchMaintenanceSelectedGen, AllGeneratorSchedules);
                OnPropertyChanged(nameof(SchMaintenanceReminderHours));

                try
                {
                    SchMaintenanceSelectedReminderLevel = UnitOfWork.GeneratorScheduler.GetReminderLevel(SchMaintenanceSelectedGen, AllGeneratorSchedules);
                    OnPropertyChanged(nameof(SchMaintenanceSelectedReminderLevel));

                    SchMaintenanceSelectedAuthorizer = UnitOfWork.GeneratorScheduler.GetAuthorizer(SchMaintenanceSelectedGen, AllGeneratorSchedules);
                    OnPropertyChanged(nameof(SchMaintenanceSelectedAuthorizer));
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
                            if (cmbxUnschMaintenance.Text == null || cmbxUnschMaintenance.Text == "")
                            {
                                MessageBox.Show("Select a generator!", "Error", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }
                            UnitOfWork.GeneratorMaintenance.AddUnschMaintenance("Unscheduled", UnschMaintenanceDate,
                                UnschMaintenanceComments, UnschMaintenanceTotalCost);
                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                                MessageBox.Show($"Unscheduled maintenance record for {cmbxUnschMaintenance.Text} added!", 
                                    "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
                            if (cmbxSchMaintenance.Text == null || cmbxSchMaintenance.Text == "")
                            {
                                MessageBox.Show("Select a generator!", "Error", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }
                            UnitOfWork.GeneratorMaintenance.AddSchMaintenance("Scheduled", SchMaintenanceDate,
                                SchMaintenanceComments, SchMaintenanceTotalCost);
                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                                MessageBox.Show($"Scheduled maintenance record for {cmbxSchMaintenance.Text} added!",
                                    "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _editReminderNotificationCmd;
        public ICommand EditReminderNotificationCmd
        {
            get
            {
                return this._editReminderNotificationCmd ??
                (
                    this._editReminderNotificationCmd = new DelegateCommand
                    (
                        x =>
                        {
                            foreach (var item in (dynamic)x)
                            {
                                if (item is TextBox)
                                {
                                    TextBox txtBxReminderNotification = item as TextBox;
                                    txtBxReminderNotification.IsReadOnly = false;
                                }
                            }
                            MessageBox.Show("Reminder & Notification are now editable", "Information", MessageBoxButton.OK,
                                            MessageBoxImage.Information);
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _activeReminderNotificationCmd;
        public ICommand ActivateReminderNotificationCmd
        {
            get
            {
                return this._activeReminderNotificationCmd ??
                (
                    this._activeReminderNotificationCmd = new DelegateCommand
                    (
                        x =>
                        {
                            string ReminderLevel = (string)SchMaintenanceSelectedReminderLevel;
                            string Authorizer = (string)SchMaintenanceSelectedAuthorizer;
                            
                            UnitOfWork.GeneratorScheduler.ActivateReminderNotification(SchMaintenanceSelectedGen, 
                                                                        SchMaintenanceStartDate, SchMaintenanceReminderHours,
                                                                        ReminderLevel, Authorizer);
                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                            {
                                MessageBox.Show($"Scheduled maintenance reminder information for {SchMaintenanceSelectedGen} has been updated!",
                                    "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                AllGeneratorSchedules = UnitOfWork.GeneratorScheduler.GetAllGeneratorSchedules();
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
