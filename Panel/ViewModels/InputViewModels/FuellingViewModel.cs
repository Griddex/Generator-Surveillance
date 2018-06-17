using Panel.Commands;
using Panel.Interfaces;
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

namespace Panel.ViewModels.InputViewModels
{
    public class FuellingViewModel : ViewModelBase, IViewModel
    {
        public ObservableCollection<GeneratorNameModel> UniqueGeneratorNames { get; set; } = new ObservableCollection<GeneratorNameModel>();

        public FuellingViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            UniqueGeneratorNames = unitOfWork.GeneratorInformation.GetUniqueGeneratorNames();
            _allGeneratorFuelConsumptionRecords = UnitOfWork.GeneratorRunningHr.GetAllRunningHours();
        }

        public UnitOfWork UnitOfWork { get; set; }
        public DateTime FuellingDate { get; set; } = DateTime.Now;

        private ObservableCollection<GeneratorRunningHr> _allGeneratorFuelConsumptionRecords;
        public ObservableCollection<GeneratorRunningHr> AllGeneratorFuelConsumptionRecords
        {
            get => _allGeneratorFuelConsumptionRecords;
            set
            {
                _allGeneratorFuelConsumptionRecords = value;
                OnPropertyChanged(nameof(AllGeneratorFuelConsumptionRecords));
            }
        }

        private string _vendor;
        public string Vendor
        {
            get => _vendor;
            set
            {
                _vendor = value;
                OnPropertyChanged(nameof(Vendor));
                ValidateVendorNameRule.ValidateVendorNameAsync(Vendor)
                    .ContinueWith(t =>
                    {
                        lock (_errors)
                        {
                            _errors.Clear();
                            _errors[nameof(Vendor)] = t.Result;
                            OnErrorsChanged(nameof(Vendor));
                        }
                    });                
            }
        }

        private double _volumeOfDiesel;
        public double VolumeOfDiesel
        {
            get => _volumeOfDiesel;
            set
            {
                _volumeOfDiesel = value;
                OnPropertyChanged(nameof(VolumeOfDiesel));
                ValidateFuelVolumeCostRule.ValidateFuelVolumeCostAsync(VolumeOfDiesel)
                    .ContinueWith(t =>
                    {
                        lock (_errors)
                        {
                            _errors.Clear();
                            _errors[nameof(VolumeOfDiesel)] = t.Result;
                            OnErrorsChanged(nameof(VolumeOfDiesel));
                        }
                    });
            }
        }

        private double _costOfDiesel;
        public double CostOfDiesel
        {
            get => _costOfDiesel;
            set
            {
                _costOfDiesel = value;
                OnPropertyChanged(nameof(CostOfDiesel));
                ValidateFuelVolumeCostRule.ValidateFuelVolumeCostAsync(CostOfDiesel)
                    .ContinueWith(t =>
                    {
                        lock (_errors)
                        {
                            _errors.Clear();
                            _errors[nameof(CostOfDiesel)] = t.Result;
                            OnErrorsChanged(nameof(CostOfDiesel));
                        }
                    });
            }
        }

        public DateTime RunningHoursDate { get; set; } = DateTime.Now;

        private double _runningHours;
        public double RunningHours
        {
            get => _runningHours;
            set
            {
                _runningHours = value;
                OnPropertyChanged(nameof(RunningHours));
                OnPropertyChanged(nameof(AllGeneratorFuelConsumptionRecords));
                ValidateRunningHoursRule.ValidateRunningHoursRuleAsync(RunningHours)
                    .ContinueWith(t =>
                    {
                        lock (_errors)
                        {
                            _errors.Clear();
                            _errors[nameof(RunningHours)] = t.Result;
                            OnErrorsChanged(nameof(RunningHours));
                        }
                    });
            }
        }

        private double _cumFuelVolumeSinceLastReading;
        public double CumFuelVolumeSinceLastReading
        {
            get => _cumFuelVolumeSinceLastReading;
            set
            {
                _cumFuelVolumeSinceLastReading = value;
                OnPropertyChanged(nameof(CumFuelVolumeSinceLastReading));
                OnPropertyChanged(nameof(AllGeneratorFuelConsumptionRecords));
                ValidateFuelVolumeCostRule.ValidateFuelVolumeCostAsync(CumFuelVolumeSinceLastReading)
                    .ContinueWith(t =>
                    {
                        lock (_errors)
                        {
                            _errors.Clear();
                            _errors[nameof(CumFuelVolumeSinceLastReading)] = t.Result;
                            OnErrorsChanged(nameof(CumFuelVolumeSinceLastReading));
                        }
                    });
            }
        }

        private bool _RequestUpdate;
        public bool RequestUpdate
        {
            get { return _RequestUpdate; }
            set
            {
                _RequestUpdate = value;
                OnPropertyChanged(nameof(RequestUpdate));
                _RequestUpdate = false;
            }
        }


        private ICommand _addPurchaseCmd;
        public ICommand AddPurchaseCmd
        {
            get
            {
                return this._addPurchaseCmd ??
                (
                    this._addPurchaseCmd = new DelegateCommand
                    (
                        x =>
                        {
                            UnitOfWork.GeneratorFuelling.AddFuelPurchaseRecord(FuellingDate, Vendor, 
                                                                        VolumeOfDiesel, CostOfDiesel);
                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                            {                                
                                MessageBox.Show("Fuel purchase added!", "Information", MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                            }                                
                            return;
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _addConsumptionCmd;
        public ICommand AddConsumptionCmd
        {
            get
            {
                return this._addConsumptionCmd ??
                (
                    this._addConsumptionCmd = new DelegateCommand
                    (
                        x =>
                        {
                            ComboBox cmbxSelectGenFuelling = x as ComboBox;
                            if(cmbxSelectGenFuelling.Text == null || cmbxSelectGenFuelling.Text == "")
                            {
                                MessageBox.Show("Select a generator!", "Error", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }
                            UnitOfWork.GeneratorFuelling.AddFuelConsumptionHours(cmbxSelectGenFuelling.Text, RunningHoursDate, 
                                                                RunningHours, CumFuelVolumeSinceLastReading);
                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                            {
                                _allGeneratorFuelConsumptionRecords = UnitOfWork.GeneratorRunningHr.GetAllRunningHours();
                                MessageBox.Show($"Fuel Consumption for {cmbxSelectGenFuelling.Text} added!", "Information", MessageBoxButton.OK,
                                   MessageBoxImage.Information);
                                RequestUpdate = true;
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
