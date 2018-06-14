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
        }

        public UnitOfWork UnitOfWork { get; set; } 

        public DateTime FuellingDate { get; set; } = DateTime.Now;

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
                            UnitOfWork.GeneratorFuelling.AddFuelPurchaseRecord(FuellingDate, Vendor, VolumeOfDiesel, CostOfDiesel);
                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                                MessageBox.Show("Fuel purchase added!", "Information", MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                            return;
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _addHoursCmd;
        public ICommand AddHoursCmd
        {
            get
            {
                return this._addHoursCmd ??
                (
                    this._addHoursCmd = new DelegateCommand
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
                            UnitOfWork.GeneratorFuelling.AddFuelConsumptionHours(cmbxSelectGenFuelling.Text, RunningHoursDate, RunningHours);
                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                                MessageBox.Show($"Fuel Consumption for {cmbxSelectGenFuelling.Text} added!", "Information", MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                            return;
                        },
                        y => !HasErrors
                    )
                );
            }
        }

    }
}
